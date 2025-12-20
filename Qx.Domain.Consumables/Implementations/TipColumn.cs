using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Column of tips
/// </summary>
public sealed class TipColumn : IConsumable
{
    private IList<ITip> _tips;
    private int _uses;
    private ConsumableStates _state;
    private ConsumableTypes? _tipType;
    
    public TipColumn(int columnIndex, int rowCount, ReusePolicy reusePolicy)
    {
        if (columnIndex < 0) throw new ArgumentOutOfRangeException(nameof(columnIndex), $"Tip column index must be greater than or equal to zero.");
        if (rowCount <= 0) throw new ArgumentOutOfRangeException(nameof(rowCount), $"Number of rows must be greater than zero");
        Name = ConsumableNamingUtility.CreateColumnName(columnIndex);
        UniqueIdentifier = Guid.NewGuid();
        Type = ConsumableTypes.TipColumn;
        _state = ConsumableStates.Available; // available because it is free for tips (in use means it has tips in it)
        ReusePolicy = reusePolicy;
        _uses = 0;
        _tips = new List<ITip>();
        Location = new Location(Name, new ColumnPosition(columnIndex));
        ColumnIndex = columnIndex;
        RowCount = rowCount;
        _tipType = null;
    }
    
    public int ColumnIndex { get; }
    public int RowCount { get; }
    public int TipCount => _tips.Count;
    public ConsumableTypes? TipType => _tipType;

    /// <summary>
    /// Add new tips to the tip column
    /// </summary>
    /// <param name="tip">Tip to be added to the column by replication</param>
    public void AddNewTips(ITip tip)
    {
        if (ContainsTips())
            throw new InvalidOperationException($"Cannot add tips to this column ({Name}), it is already in use.");
        for (int i = 0; i < TipCount; i++)
            _tips.Add(tip.ShallowCopy());
        _state = ConsumableStates.InUse;
        _tipType = tip.Type;
    }

    /// <summary>
    /// Add tips to the tip column
    /// </summary>
    /// <param name="tips">Column of tips to be added</param>
    /// <exception cref="InvalidOperationException">If tips are already in the column</exception>
    public void AddTips(TipColumn tips)
    {
        if (ContainsTips())
            throw new InvalidOperationException($"Cannot add tips to this column ({Name}), it is already in use.");
        _tips = tips.ToList();
        _state = ConsumableStates.InUse;
        _tipType = _tips[0].Type;
    }

    /// <summary>
    /// Add tips to the tip column
    /// </summary>
    /// <param name="tips">List of tips to add</param>
    /// <exception cref="InvalidOperationException">If tips are already in the column</exception>
    public void AddTips(IList<ITip> tips)
    {
        if (ContainsTips())
            throw new InvalidOperationException($"Cannot add tips to this column ({Name}), it is already in use.");
        _tips = tips;
        _state = ConsumableStates.InUse;
    }

    /// <summary>
    /// Remove the tips from the tip column
    /// </summary>
    /// <exception cref="InvalidOperationException">Exception if tips are removed when there are no tips</exception>
    /// <returns>the list of tips</returns>
    public IList<ITip> RemoveTips()
    {
        if (IsEmpty())
            throw new InvalidOperationException($"Cannot remove tips, there are no tips to remove");
        if (!ReusePolicy.CanUse(_uses))
            throw new OutOfUsesException(_uses, ReusePolicy.MaxUses.Value);
        
        var tips = _tips;
        _tips = new List<ITip>();
        _uses += 1;
        _state = _uses > ReusePolicy.MaxUses ? ConsumableStates.Consumed : ConsumableStates.Available;
        return tips;
    }

    public string Name { get; }
    public Guid UniqueIdentifier { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State => _state;
    public ReusePolicy ReusePolicy { get; }
    public int Uses => _uses;
    public Location Location { get; }

    public bool ContainsTips()
    {
        return _state == ConsumableStates.InUse;
    }

    public bool IsEmpty()
    {
        return _state == ConsumableStates.Available;
    }

    public IList<ITip> ToList()
    {
        return _tips;
    }
}