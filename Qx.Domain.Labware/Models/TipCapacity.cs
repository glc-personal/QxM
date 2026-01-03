using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Labware.Models;

public sealed record TipCapacity(Volume MaxVolume)
{
    public static TipCapacity Create(TipType tipType)
    {
        switch (tipType)
        {
            case TipType.Ul50:
                return new TipCapacity(new Volume(50d, VolumeUnits.Ul));
            case TipType.Ul200:
                return new TipCapacity(new Volume(200d, VolumeUnits.Ul));
            case TipType.Ul1000:
                return new TipCapacity(new Volume(1000d, VolumeUnits.Ul));
            default:
                throw new ArgumentException($"Invalid tip type: {tipType}");
        }
    }
}