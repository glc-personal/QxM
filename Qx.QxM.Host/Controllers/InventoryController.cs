using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;

namespace Qx.QxM.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryAppService _appService;

        public InventoryController(IInventoryAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Get the inventory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<InventoryDto>> GetInventory()
        {
            var inventory = await _appService.GetInventoryAsync();
            return Ok(inventory);
        }
    }
}
