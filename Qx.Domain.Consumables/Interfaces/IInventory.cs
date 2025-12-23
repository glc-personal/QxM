using Qx.Core;

namespace Qx.Domain.Consumables.Interfaces;

public interface IInventory : IUniquelyIdentifiable
{
    IReadOnlyList<IConsumable> Consumables { get; }
    void AddConsumable(IConsumable consumable);
    //void UpdateConsumable(Guid id, IConsumable consumable);
    void RemoveConsumable(Guid consumableId);
}