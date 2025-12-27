using Qx.Domain.Consumables.Records;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Utilities;

namespace Qx.Domain.Consumables.Exceptions;

public class InvalidTipCapacityException(VolumeContainerCapacity capacity) 
    : ArgumentException($"Invalid tip capacity ({capacity.Maximum}) must be one of [{GetValidTips()}]")
{
    private static string GetValidTips()
    {
        return string.Join(", ", TipUtility.ValidTipTypes.Select(GrabTipTypesCapacityAsString));
    }

    private static string GrabTipTypesCapacityAsString(TipTypes tipType)
    {
        if (!TipUtility.ValidTipTypes.Contains(tipType))
            throw new ArgumentException($"Invalid {nameof(TipTypes)} ({tipType}) when checking for valid tip capacity");
        return tipType.ToString().Replace("Tip", "");
    }
}