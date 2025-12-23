using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Infrastructure.Persistence.Contexts;
using Qx.Infrastructure.Persistence.Utilities;

namespace Qx.Infrastructure.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly QxMDbContext _dbContext;

    public InventoryRepository(QxMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<IInventory> GetInventoryAsync(CancellationToken cancellationToken = default)
    {
        var inventoryEntity = _dbContext.Inventory;
        //var inventory = DomainMappingUtility.MapToDomain(inventoryEntity.EntityType);
        throw new NotImplementedException();
    }

    public Task AddConsumableAsync(IConsumable consumable, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ResetInventoryAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}