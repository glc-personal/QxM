namespace Qx.Domain.Consumables.Interfaces;

public interface IInventoryRepository
{
    Task<IInventory> GetInventoryAsync(CancellationToken cancellationToken = default);
    Task AddConsumableAsync(IConsumable consumable, CancellationToken cancellationToken = default);
    //Task UpdateConsumableAsync(IConsumable consumable, CancellationToken cancellationToken = default);
    Task DeleteConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default);
    Task ResetInventoryAsync(CancellationToken cancellationToken = default);
}