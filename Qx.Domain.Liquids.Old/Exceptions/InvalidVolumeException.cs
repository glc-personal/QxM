using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Exceptions;

public class InvalidVolumeException(Volume volume) : ArgumentException($"Invalid volume ({volume.ToString()})")
{
}