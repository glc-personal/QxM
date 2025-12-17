using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Implementations;
using Qx.Domain.Liquids.Records;

namespace Qx.Tests.Domain;

public class LiquidTests
{
    [SetUp]
    public void Setup()
    {
        var water = new LiquidClass(new Viscosity(0.89, ViscosityLevels.Low),
            new SurfaceTension(72.8, SurfaceTensionLevels.High),
            VolatilityLevels.Low,
            false,
            false,
            new LiquidHandlingPolicy(false,
                false,
                null,
                null,
                null,
                1)
            );
    }

    [Test]
    public void VolumeConversionTest()
    {
        // ensure conversion to the same units doesn't change the volume
        var waterVolumeUl = new Volume(200.0, VolumeUnits.Ul);
        var waterVolume = waterVolumeUl.ToUnits(VolumeUnits.Ul);
        Assert.That(waterVolume, Is.EqualTo(waterVolumeUl));
        
        // ensure converting to a different unit changes the volume
        waterVolume = waterVolumeUl.ToUnits(VolumeUnits.Ml);
        Assert.That(waterVolume, Is.EqualTo(new Volume(0.2, VolumeUnits.Ml)));
    }

    [Test]
    public void VolumeAdditionTest()
    {
        var volume1 = new Volume(200.0, VolumeUnits.Ul);
        var volume2 = new Volume(300.0, VolumeUnits.Ul);
        var volume3 = volume1 + volume2;
        Assert.That(volume3, Is.EqualTo(volume1 + volume2));
        
        var volume4 = volume1.ToUnits(VolumeUnits.Ml);
        Assert.Catch<InvalidOperationException>(() =>
        {
            var badVolume = volume1 + volume4;
        });
    }
}