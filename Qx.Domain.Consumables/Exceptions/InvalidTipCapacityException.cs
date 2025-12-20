using Qx.Domain.Consumables.Records;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Utilities;

namespace Qx.Domain.Consumables.Exceptions;

public class InvalidTipCapacityException(VolumeContainerCapacity capacity) 
    : ArgumentException($"Invalid tip capacity ({capacity.Maximum}) must be one of [{GetValidTips()}]")
{
    private static string GetValidTips()
    {
        return string.Join(", ", TipUtility.ValidTipTypes.Select(CleanUpConsumableName));
    }

    private static string CleanUpConsumableName(ConsumableTypes consumableType)
    {
        if (!TipUtility.ValidTipTypes.Contains(consumableType))
            throw new ArgumentException($"Invalid consumable type ({consumableType}) when checking for valid tip capacity");
        return consumableType.ToString().Replace("Tip", "");
    }
}