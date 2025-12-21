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
    /// Get a plate by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IPlate> GetPlateAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get a tip box by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ITipBox> GetTipBoxAsync(Guid id, CancellationToken cancellationToken = default);
    
    //Task UpdateInventoryAsync(Guid id, CancellationToken cancellationToken = default);
}