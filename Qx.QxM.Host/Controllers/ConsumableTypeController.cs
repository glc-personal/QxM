using Microsoft.AspNetCore.Mvc;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Domain.Consumables.Enums;
using Qx.QxM.Host.Dtos;

namespace Qx.QxM.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumableTypeController : ControllerBase
    {
        private readonly IConsumableTypeAppService _appService;

        public ConsumableTypeController(IConsumableTypeAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ConsumableTypeDto>>> GetConsumableTypesAsync(
            [FromQuery] ConsumableTypes? type = null)
        {
            var consumableTypes = await _appService.GetConsumableTypesAsync(type);
            return Ok((List<ConsumableTypeDto>)consumableTypes);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddConsumableTypeAsync([FromForm] ConsumableTypes type, 
            [FromForm] VersionDto version, [FromForm] ConsumableGeometryDto geometry, [FromForm] ReusePolicyDto reusePolicy)
        {
            var consumableTypeDto = new ConsumableTypeDto
            {
                Id = Guid.NewGuid(),
                Type = type,
                Version = version.ToString(),
                ColumnCount = geometry.ColumnCount,
                RowCount = geometry.RowCount,
                ColumnOffsetMm = geometry.ColumnOffsetMm,
                RowOffsetMm = geometry.RowOffsetMm,
                LengthMm = geometry.LengthMm,
                WidthMm = geometry.WidthMm,
                HeightMm = geometry.HeightMm,
                FirstColumnOffsetMm = geometry.FirstColumnOffsetMm,
                HeightOffDeckMm = geometry.HeightOffDeckMm,
                DefaultIsReusable = reusePolicy.IsReusable,
                DefaultMaxUses = reusePolicy.MaxUses,
            };
            await _appService.AddConsumableTypeAsync(consumableTypeDto);
            return Ok();
        }
        
        [HttpDelete("{consumableTypeId}")]
        public async Task<IActionResult> RemoveConsumableTypeAsync([FromBody] Guid consumableTypeId)
        {
            await _appService.DeleteConsumableTypeAsync(consumableTypeId);
            return Ok();
        }
    }
}
