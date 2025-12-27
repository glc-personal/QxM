using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Enums;

namespace Qx.Application.Services.Interfaces;

public interface IConsumableTypeAppService
{
    /// <summary>
    /// Get a list of all consumable types
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<ConsumableTypeDto>> GetConsumableTypesAsync(ConsumableTypes? type, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get the consumable type by id
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ConsumableTypeDto> GetConsumableTypeByIdAsync(Guid typeId, CancellationToken cancellationToken = default);
    
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