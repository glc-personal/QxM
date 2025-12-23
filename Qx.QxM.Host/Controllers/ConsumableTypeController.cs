using Microsoft.AspNetCore.Mvc;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;

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
        public async Task<ActionResult<IList<ConsumableTypeDto>>> GetConsumableTypesAsync()
        {
            var consumableTypes = await _appService.GetConsumableTypesAsync();
            return (List<ConsumableTypeDto>)consumableTypes;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddConsumableTypeAsync([FromBody] ConsumableTypeDto consumableTypeDto)
        {
            await _appService.AddConsumableTypeAsync(consumableTypeDto);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveConsumableTypeAsync([FromBody] Guid consumableTypeId)
        {
            await _appService.DeleteConsumableTypeAsync(consumableTypeId);
            return Ok();
        }
    }
}
