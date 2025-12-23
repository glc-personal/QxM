namespace Qx.Domain.Consumables.Interfaces;

public interface IConsumableTypeRepository
{
    Task<IList<IConsumableType>> GetConsumableTypesAsync(CancellationToken cancellationToken = default);
    Task AddConsumableTypeAsync(IConsumableType consumableType, CancellationToken cancellationToken = default);
    Task RemoveConsumableTypeAsync(Guid consumableTypeId, CancellationToken cancellationToken = default);
    Task UpdateConsumableTypeAsync(IConsumableType consumableType, CancellationToken cancellationToken = default);
}