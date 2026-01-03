using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.LabwareInstances;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.Policies;
using Qx.Domain.Labware.States;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;
using Version = Qx.Core.Version;

namespace Qx.Tests.Domain.Labware;

[TestFixture]
public class LiquidContainerLabwareTests
{
    private LabwareDefinition _labwareDefinition;
    private LiquidContainerLabware _liquidContainerLabware;
    private IEnumerable<Volume> _aspirationVolumes;
    private IEnumerable<Volume> _dispenseVolumes;

    [SetUp]
    public void Setup()
    {
        var columnCount = 4;
        var name = "Deep Well";
        var kind = LabwareKind.LiquidContainer;
        var version = new Version(1,0,0,0);
        var envelope = new LabwareEnvelope(new Mm(230m), new Mm(140m), new Mm(440m));
        var grid = new LabwareGrid(8, columnCount, new Mm(17.5m), 
            new Mm(57.5m), new Mm(2m));
        var geometry = new LabwareGeometry(envelope, grid);
        var columnDefinitions = new List<WellColumnDefinition>();
        for (int i = 0; i < columnCount; i++)
        {
            var def = new WellColumnDefinition(i,
                new WellDefinition(WellShape.Rectangle, WellBottom.Flat,
                    new WellCapacity(new Volume(10000, VolumeUnits.Ul))),
                new SealPolicy(true));
            columnDefinitions.Add(def);
        }

        var model = LiquidContainerModel.Create(grid, columnDefinitions);
        _labwareDefinition = LabwareDefinition.Create(name, kind, version, geometry, null, model, null, LabwareId.New());
        _liquidContainerLabware = LiquidContainerLabware.Create(_labwareDefinition, _labwareDefinition.Id);
        _aspirationVolumes = Enumerable.Repeat(new Volume(200D, VolumeUnits.Ul), grid.RowCount);
        _dispenseVolumes = Enumerable.Repeat(new Volume(500D, VolumeUnits.Ul), grid.RowCount);
    }

    /// <summary>
    /// Domain Invariant: Cannot aspirate (remove volume) from an empty well column
    /// </summary>
    [Test]
    public void LiquidContainerLabwareAspirationOnEmptyWellsTests()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
                _liquidContainerLabware.AspirateFromColumn(column.ColumnIndex, _aspirationVolumes.ToList());
        });
    }

    /// <summary>
    /// Domain Invariant: Cannot aspirate (remove volume) from a sealed well column
    /// </summary>
    [Test]
    public void LiquidContainerLabwareAspirateFromSealedColumnTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
            {
                if (!column.SealPolicy.IsSealed)
                    continue;
                _liquidContainerLabware.AspirateFromColumn(column.ColumnIndex, _aspirationVolumes.ToList());
            }
        });
    }

    /// <summary>
    /// Domain Invariant: cannot dispense on a sealed well column
    /// </summary>
    [Test]
    public void LiquidContainerLabwareDispenseOnSealedColumnTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
            {
                if (!column.SealPolicy.IsSealed)
                    continue;
                _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
            }
        });
    }

    /// <summary>
    /// Domain Invariant: Cannot exceed well capacity
    /// </summary>
    [Test]
    public void LiquidContainerLabwareDispenseBeyondCapacityTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
            {
                if (_liquidContainerLabware.GetColumnSealState(column.ColumnIndex).Kind != SealStateKind.Pierced)
                    _liquidContainerLabware.PierceColumnSeal(column.ColumnIndex);
                for (int i = 0; i < 21; i++)
                {
                    _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
                    _liquidContainerLabware.GetWellVolume(new WellAddress(0, 0));
                }
            }
        });
    }
    /// <summary>
    /// Domain Invariant: cannot aspirate more than what is in the well
    /// </summary>
    [Test]
    public void LiquidContainerLabwareAspirateBeyondWellVolumeTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        var rowCount = _labwareDefinition.LiquidContainerModel.RowCount;
        var _overAspirationVolumes = Enumerable.Repeat(new Volume(550D, VolumeUnits.Ul), rowCount);
        Assert.Catch<InvalidOperationException>(() =>
        {
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
            {
                if (_liquidContainerLabware.GetColumnSealState(column.ColumnIndex).Kind != SealStateKind.Pierced)
                    _liquidContainerLabware.PierceColumnSeal(column.ColumnIndex);
                _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
                // aspirate more than what is in the well (500 uL vs the 550 uL aspiration for example)
                _liquidContainerLabware.AspirateFromColumn(column.ColumnIndex, _overAspirationVolumes.ToList());
            }
        });
    }
    
    /// <summary>
    /// Domain Invariant: cannot aspirate or dispense from an invalid column index
    /// </summary>
    [Test]
    public void LiquidContainerLabwareInvalidColumnIndexTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        var invalidColumnIndex = -1;
        Assert.Catch<ArgumentOutOfRangeException>(() =>
        {
            var _ = _labwareDefinition.LiquidContainerModel.Columns[invalidColumnIndex];
            foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
            {
                if (_liquidContainerLabware.GetColumnSealState(column.ColumnIndex).Kind != SealStateKind.Pierced)
                    _liquidContainerLabware.PierceColumnSeal(column.ColumnIndex);
                _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
                for (int rowIndex = 0; rowIndex < _labwareDefinition.LiquidContainerModel.RowCount; rowIndex++)
                    _liquidContainerLabware.GetWellVolume(new WellAddress(rowIndex, column.ColumnIndex));
                _liquidContainerLabware.AspirateFromColumn(column.ColumnIndex, _aspirationVolumes.ToList());
            }
        });
    }
    
    /// <summary>
    /// Domain Invariant: accurate volume tracking
    /// </summary>
    [Test]
    public void LiquidContainerLabwareAccurateVolumeTrackingTest()
    {
        Assert.NotNull(_labwareDefinition.LiquidContainerModel);
        foreach (var column in _labwareDefinition.LiquidContainerModel.Columns)
        {
            if (_liquidContainerLabware.GetColumnSealState(column.ColumnIndex).Kind != SealStateKind.Pierced)
                _liquidContainerLabware.PierceColumnSeal(column.ColumnIndex);
            _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
            for (int rowIndex = 0; rowIndex < _labwareDefinition.LiquidContainerModel.RowCount; rowIndex++)
                Assert.That(_dispenseVolumes.ToList()[rowIndex], Is.EqualTo(_liquidContainerLabware.GetWellVolume(new WellAddress(rowIndex, column.ColumnIndex))));
            _liquidContainerLabware.AspirateFromColumn(column.ColumnIndex, _aspirationVolumes.ToList());
            for (int rowIndex = 0; rowIndex < _labwareDefinition.LiquidContainerModel.RowCount; rowIndex++)
            {
                var wellVolume = _liquidContainerLabware.GetWellVolume(new WellAddress(rowIndex, column.ColumnIndex));
                var expectedVolume = _dispenseVolumes.ToList()[rowIndex] - _aspirationVolumes.ToList()[rowIndex];
                Assert.That(wellVolume, Is.EqualTo(expectedVolume));
            }
        }
    }
}