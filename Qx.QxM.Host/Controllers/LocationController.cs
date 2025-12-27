using Microsoft.AspNetCore.Mvc;
using Qx.Application.Services.Interfaces;
using Qx.Domain.Consumables.Utilities;
using LocationDto = Qx.Application.Services.Dtos.LocationDto;

namespace Qx.QxM.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationAppService _locationAppService;

        public LocationController(ILocationAppService locationAppService)
        {
            _locationAppService = locationAppService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationDto>>> GetLocationsAsync([FromQuery] string? locationName = null)
        {
            var locations = await _locationAppService.GetLocationsAsync(locationName);
            return Ok(locations);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddLocationAsync([FromForm] Dtos.LocationDto locationDto)
        {
            var dto = new LocationDto
            {
                Id = Guid.NewGuid(),
                Name = ConsumableNamingUtility.CreateConsumableName(locationDto.Slot,
                    locationDto.Batch,
                    locationDto.ColumnIndex),
                Frame = locationDto.Frame.ToString(),
                XUs = locationDto.Coordinate.X,
                YUs = locationDto.Coordinate.Y,
                ZUs = locationDto.Coordinate.Z,
            };
            await _locationAppService.AddLocationAsync(dto);
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteLocationAsync(Guid id)
        {
            await _locationAppService.DeleteLocationAsync(id);
            return Ok();
        }
    }
}
