using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Utilities;

public static class VolumeContainerUtility
{
    public static IList<VolumeContainerCapacity> ConvertDoublesToCapacities(IList<double> input, VolumeUnits units)
    {
        IList<VolumeContainerCapacity> capacities = new List<VolumeContainerCapacity>();
        foreach (var value in input)
            capacities.Add(new VolumeContainerCapacity(new Volume(value, units)));
        return capacities;
    }
}