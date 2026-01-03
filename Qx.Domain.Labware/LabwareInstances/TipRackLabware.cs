using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.Policies;
using Qx.Domain.Labware.States;

namespace Qx.Domain.Labware.LabwareInstances;

public sealed class TipRackLabware : Labware
{
    private TipRackModel _model;
    // TODO: replace with ReuseStateMachine and TipStateMachine
    private readonly Dictionary<int, ReuseState> _reuseStates = new Dictionary<int, ReuseState>();
    private readonly Dictionary<int, TipState> _tipStates = new Dictionary<int, TipState>();

    private TipRackLabware(LabwareDefinition definition, List<int> columnIndexesWithTips, LabwareId? id = null) 
        : base(definition, id)
    {
        _model = definition.TipModel ?? throw new ArgumentNullException($"{nameof(definition.TipModel)} cannot be null for {nameof(TipRackLabware)}");
        ArgumentNullException.ThrowIfNull(definition.Geometry.Grid, $"{nameof(definition.Geometry.Grid)} cannot be null for {nameof(TipRackLabware)}");
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            _reuseStates.Add(columnIndex, column.ReusePolicy.IsReusable 
                ? ReuseState.New(ReuseStateKind.Reusable) : ReuseState.New(ReuseStateKind.NotReusable));
            _tipStates.Add(columnIndex, columnIndexesWithTips.Contains(columnIndex) 
                ? TipState.InRack(columnIndex) : TipState.NotInRack(columnIndex));
        }
    }

    public static TipRackLabware Create(LabwareDefinition definition, List<int> columnIndexesWithTips, LabwareId? id = null)
    {
        return new TipRackLabware(definition, columnIndexesWithTips, id);
    }

    public TipState GetColumnTipState(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        return _tipStates[columnIndex];
    }

    public ReuseState GetColumnReuseState(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        return _reuseStates[columnIndex];
    }

    public ReusePolicy GetColumnReusePolicy(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        return _model.Columns[columnIndex].ReusePolicy;
    }

    public void PickUpTipColumn(int columnIndex)
    {
        EnforceUnusableLifecycleState();
        // TODO: consider raising an event for the reuse state?
        EnforceValidColumnIndex(columnIndex);
        EnforceValidPickUp(columnIndex);
        var columnTipState = GetColumnTipState(columnIndex);
        if (columnTipState.Kind == TipStateKind.NotInRack) return;
        EnforceReuse(columnIndex);
        _tipStates[columnIndex] = TipState.EngagedToPipetteHead(columnIndex);
    }

    public void EjectTipColumn(int columnIndex, bool incrementUse)
    {
        EnforceUnusableLifecycleState();
        EnforceValidColumnIndex(columnIndex);
        // TODO: could ignore this domain invariant and use a tip parking spot service to determine the best place to put the tips (more complicated and not really worth it?)
        EnforceUniqueTipColumnEject(columnIndex);
        if (_tipStates[columnIndex].Kind != TipStateKind.EngagedToPipetteHead)
            throw new InvalidOperationException($"Cannot eject tip column {columnIndex} because it is not engaged to pipette head.");
        if (incrementUse)
        {
            var reusePolicy = _model.Columns[columnIndex].ReusePolicy;
            _reuseStates[columnIndex] = _reuseStates[columnIndex].Use(reusePolicy, DateTimeOffset.UtcNow);
        }
        _tipStates[columnIndex] = TipState.InRack(columnIndex);
    }

    private void EnforceValidColumnIndex(int columnIndex)
    {
        if (_model.Columns.All(c => c.ColumnIndex != columnIndex))
            throw new ArgumentException($"The column index {columnIndex} is invalid");
    }

    private void EnforceOneTipColumnEngaged(int columnIndex)
    {
        var columnTipState = GetColumnTipState(columnIndex);
        if (columnTipState.Kind != TipStateKind.EngagedToPipetteHead) return;
        var columIndexes = _model.Columns.Select(column => column.ColumnIndex).ToList();
        foreach (var index in columIndexes.Where(index => index != columnIndex && GetColumnTipState(index).Kind == TipStateKind.EngagedToPipetteHead))
            throw new InvalidOperationException($"The column index {index} is already engaged to pipette head, only one at a time");
    }

    private void EnforceUniqueTipColumnEject(int columnIndex)
    {
        if (_tipStates[columnIndex].ColumnIndex != columnIndex)
            throw new InvalidOperationException($"Cannot eject tips in column {columnIndex} because this tip column belongs to {_tipStates[columnIndex].ColumnIndex}");
    }

    private void EnforceReuse(int columnIndex)
    {
        var reusePolicy = GetColumnReusePolicy(columnIndex);
        if (_reuseStates[columnIndex].Kind != ReuseStateKind.Reusable && _reuseStates[columnIndex].UseCount >= 1)
            throw new InvalidOperationException($"Cannot use this tip column {columnIndex} because it is {GetColumnReuseState(columnIndex).Kind}");
    }

    private void EnforceValidPickUp(int columnIndex)
    {
        EnforceOneTipColumnEngaged(columnIndex);
        if (_tipStates[columnIndex].Kind == TipStateKind.NotInRack) 
            throw new InvalidOperationException($"Cannot pick up tips in column {columnIndex} because this tip column is {_tipStates[columnIndex].Kind}");
        foreach (var column in _model.Columns)
        {
            if (_tipStates[column.ColumnIndex].Kind == TipStateKind.EngagedToPipetteHead)
            {
                throw new InvalidOperationException(
                    $"Cannot pick up tip column {columnIndex} because tip column {column.ColumnIndex} is engaged to pipette head");
            }
        }
    }
}