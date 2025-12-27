using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Application.Services.Utilities;
using Qx.Domain.Locations.Interfaces;

namespace Qx.Application.Services.Implementations;

public class LocationAppService : ILocationAppService
{
    private readonly ILocationRepository _repo;

    public LocationAppService(ILocationRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<IList<LocationDto>> GetLocationsAsync(string? locationName)
    {
        var locationDtos = new List<LocationDto>();
        var locations = await _repo.GetLocationsAsync(locationName);
        foreach (var location in locations)
        {
            if (locationName != null && locationName != location.Name) continue;
            locationDtos.Add(DtoMappingUtility.MapToDto(location));
        }
        return locationDtos;
    }

    public async Task<LocationDto> GetLocationByIdAsync(Guid locationId)
    {
        var location = await _repo.GetLocationByIdAsync(locationId);
        var locationDto = DtoMappingUtility.MapToDto(location);
        return locationDto;
    }

    public async Task AddLocationAsync(LocationDto locationDto)
    {
        await _repo.AddLocationAsync(DomainMappingUtility.MapToDomain(locationDto));
    }

    public async Task DeleteLocationAsync(Guid locationId)
    {
        await _repo.DeleteLocationAsync(locationId);
    }
}