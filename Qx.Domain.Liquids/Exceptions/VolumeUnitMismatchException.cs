using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Exceptions;

public class VolumeUnitMismatchException(Volume volume1, Volume volume2) 
    : Exception($"Volume unit mismatch between {volume1.ToString()} and {volume2.ToString()}")
{
    
}