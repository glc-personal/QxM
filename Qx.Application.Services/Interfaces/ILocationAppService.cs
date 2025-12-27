using Qx.Application.Services.Dtos;

namespace Qx.Application.Services.Interfaces;

public interface ILocationAppService
{
    Task<IList<LocationDto>> GetLocationsAsync(string? locationName);
    Task<LocationDto> GetLocationByIdAsync(Guid locationId);
    Task AddLocationAsync(LocationDto locationDto);
    //Task UpdateLocationAsync(LocationDto location);
    Task DeleteLocationAsync(Guid locationId);
}