using Qx.Core;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Interfaces;

/// <summary>
/// Consumable that can be used and reused
/// </summary>
public interface IConsumable : INameable, IUniquelyIdentifiable
{
    ConsumableTypes Type { get; }
    ConsumableStates State { get; }
    ReusePolicy ReusePolicy { get; }
    int Uses { get; }
    Location Location { get; }
}