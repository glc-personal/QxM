using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Records;

/// <summary>
/// Well Capacity 
/// </summary>
/// <param name="Maximum"></param>
public sealed record WellCapacity(Volume Maximum);