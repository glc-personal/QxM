using Qx.Domain.Consumables.Implementations;

namespace Qx.Domain.Consumables.Interfaces;

public interface IInventoryRepository
{
    Task<Inventory> GetInventoryAsync(CancellationToken cancellationToken = default);
    Task AddPlateAsync(IPlate plate, CancellationToken cancellationToken = default);
    Task AddTipBoxAsync(ITipBox tipBox, CancellationToken cancellationToken = default);
    //Task UpdatePlateAsync(Guid plateId, IPlate plate, CancellationToken cancellationToken = default);
    //Task UpdateTipBoxAsync(Guid tipBoxId, ITipBox tipBox, CancellationToken cancellationToken = default);
    Task DeletePlateAsync(Guid plateId, CancellationToken cancellationToken = default);
    Task DeleteTipBoxAsync(Guid tipBoxId, CancellationToken cancellationToken = default);
    Task DeleteInventoryAsync(CancellationToken cancellationToken = default);
}