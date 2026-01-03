using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.LabwareInstances;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.States;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;
using ReusePolicy = Qx.Domain.Labware.Policies.ReusePolicy;
using Version = Qx.Core.Version;

namespace Qx.Tests.Domain.Labware;

[TestFixture]
public class TipRackLabwareTests
{
    private TipRackModel _model;
    private TipRackLabware _tipRack;
    private LabwareDefinition _labwareDefinition;
    
    [SetUp]
    public void Setup()
    {
        var rowCount = 8;
        var columnCount = 12;
        var maxUses = 2;
        var columnThatIsNotReusable = 1;
        var name = "TipRack-A";
        var kind = LabwareKind.TipRack;
        var version = new Version(1,0,0,0);
        var envelope = new LabwareEnvelope(new Mm(230m), new Mm(140m), new Mm(32.4m));
        var grid = new LabwareGrid(rowCount, columnCount, new Mm(17.5m), 
            new Mm(57.5m), new Mm(2m));
        var geometry = new LabwareGeometry(envelope, grid);
        var columnDefinitions = new List<TipColumnDefinition>();
        var tipCapacity = TipCapacity.Create(TipType.Ul200);
        var recommendedMinVol = new Volume(2d, VolumeUnits.Ul);
        var tipLength = new Mm(71m);
        var seatedDepth = new Mm(25m);
        for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
            var tipDef = new TipDefinition(tipCapacity, recommendedMinVol, TipStyle.Standard, tipLength, seatedDepth, 
                true, false);
            var reusePolicy = columnIndex == columnThatIsNotReusable ? ReusePolicy.NonReusable() : ReusePolicy.Limited(maxUses);
            var colDef = new TipColumnDefinition(columnIndex, tipDef, reusePolicy);
            columnDefinitions.Add(colDef);
        }
        _model = TipRackModel.Create(grid, columnDefinitions);
        _labwareDefinition =
            LabwareDefinition.Create(name, kind, version, geometry, _model, null, null, LabwareId.New());
        var columnsWithTips = new List<int> { 0, 1, 2, 3, 4, 5 };
        _tipRack = TipRackLabware.Create(_labwareDefinition, columnsWithTips, _labwareDefinition.Id);
    }

    /// <summary>
    /// Domain Invariant: cannot eject tips if they are not engaged to pipette head
    /// </summary>
    [Test]
    public void TipRackLabwareCannotEjectNonEngagedTipsTest()
    {
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _model.Columns)
            {
                var columnIndex = column.ColumnIndex;
                var tipState = _tipRack.GetColumnTipState(columnIndex);
                // try ejecting any tip column that is not engaged to pipette head
                if (tipState.Kind != TipStateKind.EngagedToPipetteHead)
                    _tipRack.EjectTipColumn(columnIndex, false);
            }
        });
    }
    
    /// <summary>
    /// Domain Invariant: cannot pick up tip column from a column with no tips
    /// </summary>
    [Test]
    public void TipRackLabwareCannotPickUpEmptyColumnTest()
    {
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _model.Columns)
            {
                var columnIndex = column.ColumnIndex;
                var tipState = _tipRack.GetColumnTipState(columnIndex);
                if (tipState.Kind == TipStateKind.NotInRack)
                    _tipRack.PickUpTipColumn(columnIndex);
            }
        });
    }
    
    /// <summary>
    /// Domain Invariant: cannot pick up tip column if a tip column is already engaged to pipette head
    /// </summary>
    [Test]
    public void TipRackLabwareCannotPickUpTipsIfTipsAlreadyEngagedTest()
    {
        var columnIndex = _model.Columns[0].ColumnIndex;
        _tipRack.PickUpTipColumn(columnIndex);
        Assert.Catch<InvalidOperationException>(() => { _tipRack.PickUpTipColumn(columnIndex); });
    }
    
    /// <summary>
    /// Domain Invariant: cannot eject tip column into wrong column
    /// </summary>
    [Test]
    public void TipRackLabwareCannotEjectIntoWrongColumnTest()
    {
        var ejectIntoColumnIndex = 0;
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            var tipState = _tipRack.GetColumnTipState(columnIndex);
            if (tipState.Kind == TipStateKind.InRack && columnIndex != ejectIntoColumnIndex)
            {
                _tipRack.PickUpTipColumn(columnIndex);
                Assert.Catch<InvalidOperationException>(() => { _tipRack.EjectTipColumn(ejectIntoColumnIndex, false); });
                // eject correctly before continuing to test next column
                _tipRack.EjectTipColumn(columnIndex, false);
            }
        }
    }
    
    // Domain Invariant: invalid column indexes don't work
    [TestCase(-1)]
    [TestCase(100)]
    public void TipRackLabwareInvalidColumnIndexTest(int columnIndex)
    {
        Assert.Catch<ArgumentException>(() => { _tipRack.PickUpTipColumn(columnIndex); });
    }
    
    /// <summary>
    /// Domain Invariant: reuse is stopped when out of uses
    /// </summary>
    [Test]
    public void TipRackLabwareReuseOutOfUsesTest()
    {
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            var tipState = _tipRack.GetColumnTipState(columnIndex);
            if (tipState.Kind != TipStateKind.InRack) continue;
            if (column.ReusePolicy is not { IsReusable: true, MaxUses: not null }) continue;
            var maxUses = column.ReusePolicy.MaxUses;
            // use the tip column 1 too many times
            for (var use = 0; use <= maxUses; use++)
            {
                var reuseState = _tipRack.GetColumnReuseState(columnIndex);
                if (reuseState.Kind == ReuseStateKind.OutOfUses)
                {
                    Assert.Catch<InvalidOperationException>(() => { _tipRack.PickUpTipColumn(columnIndex); });
                    break;
                }
                _tipRack.PickUpTipColumn(columnIndex);
                _tipRack.EjectTipColumn(columnIndex, true);
            }
        }
    }
    
    // Domain Invariant: reuse isn't allowed beyond one use if not reusable
    [Test]
    public void TipRackLabwareNotReusableTest()
    {
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            var tipState = _tipRack.GetColumnTipState(columnIndex);
            if (tipState.Kind != TipStateKind.InRack) continue;
            if (column.ReusePolicy is not { IsReusable: false, MaxUses: null }) continue;
            // use the non-reusable tips one too many times
            for (var use = 0; use <= 1; use++)
            {
                var reuseState = _tipRack.GetColumnReuseState(columnIndex);
                Assert.That(reuseState.Kind == ReuseStateKind.NotReusable);
                if (use == 1)
                {
                    Assert.Catch<InvalidOperationException>(() => { _tipRack.PickUpTipColumn(columnIndex); });
                    break;
                }
                _tipRack.PickUpTipColumn(columnIndex);
                _tipRack.EjectTipColumn(columnIndex, true);
            }
        }
    }
    
    // Domain Invariant: reuse is allowed when within max uses
    
    /// <summary>
    /// Test successful tip pick up and eject
    /// </summary>
    [Test]
    public void TipRackLabwarePickUpAndEjectTest()
    {
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            var tipState = _tipRack.GetColumnTipState(columnIndex);
            if (tipState.Kind != TipStateKind.InRack) continue;
            Assert.DoesNotThrow(() =>
            {
                _tipRack.PickUpTipColumn(columnIndex);
                _tipRack.EjectTipColumn(columnIndex, false);
            });
        }
    }

    /// <summary>
    /// Test successful tip pick up and eject states are valid
    /// </summary>
    [Test]
    public void TipRackLabwarePickUpAndEjectStateTest()
    {
        foreach (var column in _model.Columns)
        {
            var columnIndex = column.ColumnIndex;
            var tipState = _tipRack.GetColumnTipState(columnIndex);
            if (tipState.Kind == TipStateKind.InRack)
            {
                _tipRack.PickUpTipColumn(columnIndex);
                tipState = _tipRack.GetColumnTipState(columnIndex);
                Assert.That(tipState.Kind, Is.EqualTo(TipStateKind.EngagedToPipetteHead));
                _tipRack.EjectTipColumn(columnIndex, false);
                tipState = _tipRack.GetColumnTipState(columnIndex);
                Assert.That(tipState.Kind, Is.EqualTo(TipStateKind.InRack));
            }
        }
    }
}