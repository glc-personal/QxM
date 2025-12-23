using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Implementations;

/// <summary>
/// Inventory application service for inventory business logic
/// </summary>
public class InventoryAppService : IInventoryAppService
{
    private readonly IInventoryRepository _repo;

    public InventoryAppService(IInventoryRepository repo)
    {
        _repo = repo;
    }
    
    /// <inheritdoc/>
    public Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task ResetInventoryAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task AddConsumableAsync(IConsumable consumable, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<IConsumable> GetConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RemoveConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}