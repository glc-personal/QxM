using Qx.Application.Services.Dtos;

namespace Qx.Application.Services.Interfaces;

public interface IConsumableTypeAppService
{
    /// <summary>
    /// Get a list of all consumable types
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<ConsumableTypeDto>> GetConsumableTypesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Add a consumable
    /// </summary>
    /// <param name="consumableTypeDto">Consumable to add</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddConsumableTypeAsync(ConsumableTypeDto consumableTypeDto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Update consumable
    /// </summary>
    /// <param name="consumableTypeDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateConsumableTypeAsync(Guid consumableTypeId, ConsumableTypeDto consumableTypeDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a consumable
    /// </summary>
    /// <param name="consumableId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteConsumableTypeAsync(Guid consumableId, CancellationToken cancellationToken = default);
}