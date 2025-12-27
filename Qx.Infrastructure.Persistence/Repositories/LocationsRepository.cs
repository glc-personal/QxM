using Microsoft.EntityFrameworkCore;
using Qx.Domain.Locations.Implementations;
using Qx.Domain.Locations.Interfaces;
using Qx.Infrastructure.Persistence.Contexts;
using Qx.Infrastructure.Persistence.Entities;
using Qx.Infrastructure.Persistence.Utilities;

namespace Qx.Infrastructure.Persistence.Repositories;

public class LocationsRepository : ILocationRepository
{
    private readonly QxMDbContext _context;

    public LocationsRepository(QxMDbContext context)
    {
        _context = context;
    }
    
    public async Task<IList<Location>> GetLocationsAsync(string? locationName, CancellationToken cancellationToken = default)
    {
        var domainLocations = new List<Location>();
        var locationEntities = await _context.Location.ToListAsync(cancellationToken);
        foreach (var locationEntity in locationEntities)
        {
            if (locationName != null && locationEntity.Name != locationName) continue;
            domainLocations.Add(DomainMappingUtility.MapToDomain(locationEntity));
        }
        return domainLocations;
    }

    public async Task<Location> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        var locationEntity = await _context.Location.FindAsync(locationId, cancellationToken);
        if (locationEntity == null) throw new KeyNotFoundException($"Location with id {locationId} not found");
        return DomainMappingUtility.MapToDomain(locationEntity);
    }

    public async Task AddLocationAsync(Location location, CancellationToken cancellationToken = default)
    {
        var locationEntity = EntityMappingUtility.MapToEntity(location);
        await _context.Location.AddAsync(locationEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteLocationAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        var locationEntity = await _context.Location.FindAsync(locationId, cancellationToken);
        if (locationEntity == null) throw new KeyNotFoundException($"Location with id {locationId} not found");
        _context.Location.Remove(locationEntity);
    }
}