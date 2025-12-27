using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Locations.Interfaces;

public interface ILocationRepository
{
    Task<IList<Location>> GetLocationsAsync(string? locatioName, CancellationToken cancellationToken = default);
    Task<Location> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default);
    Task AddLocationAsync(Location location, CancellationToken cancellationToken = default);
    //Task UpdateLocationAsync(Guid locationId, Location location);
    Task DeleteLocationAsync(Guid locationId, CancellationToken cancellationToken = default);
}