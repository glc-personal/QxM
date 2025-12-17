using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Exceptions;
using Qx.Domain.Liquids.Records;

namespace Qx.Tests.Domain;

[TestFixture]
public class ConsumablesTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void WellVolumeTests()
    {
        var well = new Well(WellTypes.Circle,
            new WellAddress(0, 0),
            new Volume(200.0, VolumeUnits.Ul),
            new VolumeContainerCapacity(
                new Volume(1000.0, VolumeUnits.Ul)
            )
        );

        var volumeValue = 50.0;
        var originalVolume = new Volume(well.Volume.Value, VolumeUnits.Ul);
        // ensure volume can be added
        var addedVolume = new Volume(volumeValue, VolumeUnits.Ul);
        well.AddVolume(addedVolume);
        Assert.That(addedVolume + originalVolume, Is.EqualTo(well.Volume));

        // ensure volume can be removed
        var removedVolume = new Volume(volumeValue, VolumeUnits.Ul);
        well.RemoveVolume(removedVolume);
        Assert.That(originalVolume, Is.EqualTo(well.Volume));
        
        // ensure we catch capacity issues
        Assert.Catch<MaximumVolumeExceededException>(() =>
        {
            addedVolume = new Volume(900.0, VolumeUnits.Ul);
            well.AddVolume(addedVolume);
        });
        
        // ensure creating a new well with a volume greater than capacity is caught
        Assert.Catch<MaximumVolumeExceededException>(() =>
        {
            var badWell = new Well(WellTypes.Circle,
                new WellAddress(0, 0),
                new Volume(200.0, VolumeUnits.Ul),
                new VolumeContainerCapacity(new Volume(
                    100.0, VolumeUnits.Ul)
                )
            );
        });
    }

    [Test]
    public void WellColumnVolumeTests()
    {
        var wells = SetupWells();
        var wellColumn = SetupWellColumn(wells);
        Volume[] volumes = [new Volume(200.0, VolumeUnits.Ul), new Volume(50.0, VolumeUnits.Ul)];
        
        // ensure volume can be added to the wells
        wellColumn.AddVolume(volumes);
        
        // ensure volume can be removed from the wells
        wellColumn.RemoveVolume(volumes);
        
        // ensure we can catch if the number of volumes differs from the number of wells
        Volume[] badVolumes = [new Volume(200.0, VolumeUnits.Ul), new Volume(50.0, VolumeUnits.Ul), new Volume(100.0, VolumeUnits.Ul)];
        Assert.Catch<ArgumentException>(() => wellColumn.AddVolume(badVolumes));
    }
    
    [Test]
    public void PlateVolumeTests()
    {
        var wells = SetupWells();
        var wellColumn = SetupWellColumn(wells);
        var plate = SetupPlate([wellColumn]);
        
        // ensure foil seal is pierced before adding or removing volume
        Assert.Catch<PlateFoilSealException>(() =>
        {
            plate.AddVolume([new Volume(200.0, VolumeUnits.Ul), new Volume(50.0, VolumeUnits.Ul)], 0);
        });

        // ensure adding too much excess volume to a well catches
        Assert.Catch<MaximumVolumeExceededException>(() =>
        {
            plate.PierceFoilSeal(0);
            plate.AddVolume([new Volume(1000.0, VolumeUnits.Ul), new Volume(50.0, VolumeUnits.Ul)], 0);
        });

        // ensure adding less than capacity works as expected
    }

    [Test]
    public void TipTests()
    {
        var overuseTip = SetupTip(1000.0, 2);
        var overcapacityTip = SetupTip(50.0, 2);
        
        // ensure adding volume to the tip works and that reuse works
        overuseTip.AddVolume(new Volume(200.0, VolumeUnits.Ul));
        overuseTip.AddVolume(new Volume(200.0, VolumeUnits.Ul));
        // ensure adding more volume when tip is consumed is caught
        Assert.Catch<OutOfUsesException>(() =>
        {
            overuseTip.AddVolume(new Volume(200.0, VolumeUnits.Ul));
        });

        // ensure overcapacity is caught
        Assert.Catch<MaximumVolumeExceededException>(() =>
        {
            overcapacityTip.AddVolume(new Volume(200.0, VolumeUnits.Ul));
        });
    }

    private IReadOnlyList<Well> SetupWells()
    {
        var well1 = new Well(WellTypes.Circle,
            new WellAddress(0, 0),
            new Volume(200.0, VolumeUnits.Ul),
            new VolumeContainerCapacity(new Volume(1000.0, VolumeUnits.Ul)));
        var well2 = new Well(WellTypes.Circle,
            new WellAddress(0, 0),
            new Volume(0.0, VolumeUnits.Ul),
            new VolumeContainerCapacity(new Volume(500.0, VolumeUnits.Ul)));
        return [well1, well2];
    }
    
    private WellColumn SetupWellColumn(IReadOnlyList<Well> wells)
    {
        return new WellColumn(wells);
    }
    
    private Plate SetupPlate(IReadOnlyList<WellColumn> wellColumns)
    {
        var plate = new Plate(0,
            "96 well plate",
            new ReusePolicy(false, null),
            null,
            wellColumns,
            new FoilSealPolicy(true));
        return plate;
    }

    private Tip SetupTip(double tipCapacityUl, int numberOfReuses)
    {
        return new Tip(0,
            new ReusePolicy(true, numberOfReuses),
            null,
            new VolumeContainerCapacity(new Volume(tipCapacityUl, VolumeUnits.Ul)
            )
        );
    }
}