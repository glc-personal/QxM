using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Interfaces;

public interface IInventoryAppService
{
    /// <summary>
    /// Get the current loaded inventory
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reset the inventory
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ResetInventoryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Add a consumable to the inventory
    /// </summary>
    /// <param name="consumable">Consumable to be added</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddConsumableAsync(IConsumable consumable, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get a consumable by its id
    /// </summary>
    /// <param name="consumableId">Id of the consumable</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IConsumable> GetConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default);
    
    //Task UpdateInventoryAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Remove a consumable from the inventory
    /// </summary>
    /// <param name="consumableId">Id of the consumable to be removed</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveConsumableAsync(Guid consumableId, CancellationToken cancellationToken = default);
}