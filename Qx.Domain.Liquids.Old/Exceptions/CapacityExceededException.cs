using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Exceptions;

public class CapacityExceededException(Volume volume, Volume maxVolume) 
    : Exception($"Capacity ({maxVolume.ToString()}) would be exceeded by: {(volume - maxVolume).ToString()}")
{
}