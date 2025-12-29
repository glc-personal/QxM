using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.LabwareInstances;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.Policies;
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
        var name = "Deep Well";
        var kind = LabwareKind.LiquidContainer;
        var version = new Version
        {
            Major = 1,
            Minor = 0,
            Build = 0,
            Revision = 0
        };
        var envelope = new LabwareEnvelope(new Millimeters(230m), new Millimeters(140m), new Millimeters(440m));
        var grid = new LabwareGrid(8, 4, new Millimeters(17.5m), 
            new Millimeters(57.5m), new Millimeters(2m));
        var geometry = new LabwareGeometry(envelope, grid);
        var columnDefinitions = new List<WellColumnDefinition>();
        for (int i = 0; i < 4; i++)
        {
            var def = new WellColumnDefinition(i,
                new WellDefinition(WellShape.Rectangle, WellBottom.Flat,
                    new WellCapacity(new Volume(10000, VolumeUnits.Ul))),
                new SealPolicy(true));
            columnDefinitions.Add(def);
        }

        var model = LiquidContainerModel.Create(grid, columnDefinitions);
        _labwareDefinition = LabwareDefinition.Create(name, kind, version, geometry, null, model, null, null);
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
        Assert.Catch<ArgumentException>(() =>
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
                _liquidContainerLabware.PierceColumnSeal(column.ColumnIndex);
                for (int i = 0; i < 21; i++)
                {
                    _liquidContainerLabware.DispenseToColumn(column.ColumnIndex, _dispenseVolumes.ToList());
                    _liquidContainerLabware.GetWellVolume(new WellAddress(0, 0));
                }
            }
        });
    }
    
    // Domain Invariant: cannot aspirate more than what is in the well
    // Domain Invariant: cannot aspirate or dispense from an invalid column index
    // Domain Invariant: accurate volume tracking
}