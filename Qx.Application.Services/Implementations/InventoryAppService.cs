using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Implementations;

/// <summary>
/// Inventory application service for inventory business logic
/// </summary>
public class InventoryAppService : IInventoryAppService
{
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
    public Task<IPlate> GetPlateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ITipBox> GetTipBoxAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}