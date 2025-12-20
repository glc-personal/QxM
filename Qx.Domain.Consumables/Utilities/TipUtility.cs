using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Enums;

namespace Qx.Domain.Consumables.Utilities;

public static class TipUtility
{
    public static readonly List<ConsumableTypes> ValidTipTypes =
    [
        ConsumableTypes.Tip1000Ul,
        ConsumableTypes.Tip200Ul,
        ConsumableTypes.Tip50Ul
    ];
    
    public static ConsumableTypes GetConsumableTypeFromTipCapacity(VolumeContainerCapacity capacity)
    {
        // convert the capacity volume to Ul
        var capacityVolumeUl = capacity.Maximum.ToUnits(VolumeUnits.Ul);
        foreach (var type in ValidTipTypes)
        {
            if (type.ToString().Replace("Tip", "") == capacityVolumeUl.ToString().Replace(" ", ""))
                return type;
        }

        throw new InvalidTipCapacityException(capacity);
    }
    
}