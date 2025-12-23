using Qx.Domain.Consumables.Interfaces;

namespace Qx.Domain.Consumables.Implementations;

public sealed class Inventory : IInventory
{
    private IList<IConsumable> _consumables;

    public Inventory()
    {
        _consumables = new List<IConsumable>();
        UniqueIdentifier = Guid.NewGuid();
    }
    
    public IReadOnlyList<IConsumable> Consumables => _consumables.AsReadOnly();
    public Guid UniqueIdentifier { get; }
    
    public void AddConsumable(IConsumable consumable)
    {
        if (HasConsumableByName(consumable))
            throw new InvalidOperationException("Consumable is already in use");
        _consumables.Add(consumable);
    }

    public void RemoveConsumable(Guid consumableId)
    {
        if (!HasConsumableById(consumableId))
            throw new InvalidOperationException($"Consumable (id: {consumableId}) not in the inventory");
        _consumables.Remove(_consumables.First(c => c.UniqueIdentifier == consumableId));
    }

    private bool HasConsumableByName(IConsumable consumable) 
        => _consumables.Any(c => c.Name == consumable.Name);

    private bool HasConsumableById(Guid consumableId) 
        => _consumables.Any(c => c.UniqueIdentifier == consumableId);
}