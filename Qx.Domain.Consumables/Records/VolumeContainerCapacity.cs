using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Records;

/// <summary>
/// Volume Container Capacity 
/// </summary>
/// <param name="Maximum">Maximum volume the container can hold</param>
public sealed record VolumeContainerCapacity(Volume Maximum);