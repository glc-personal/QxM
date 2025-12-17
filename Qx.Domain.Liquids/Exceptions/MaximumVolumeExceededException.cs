using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Exceptions;

public class MaximumVolumeExceededException(Volume volume, Volume maxVolume) 
    : Exception($"Maximum volume ({maxVolume.ToString()}) would be exceeded by: {(volume - maxVolume).ToString()}")
{
}