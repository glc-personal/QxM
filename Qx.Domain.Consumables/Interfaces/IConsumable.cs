using Qx.Core;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Locations.Interfaces;

namespace Qx.Domain.Consumables.Interfaces;

/// <summary>
/// Consumable that can be used and reused
/// </summary>
public interface IConsumable : IIdentifiable
{
    ConsumableTypes Type { get; }
    ConsumableStates State { get; }
    ReusePolicy ReusePolicy { get; }
    int Uses { get; }
    ILocation Location { get; }
}