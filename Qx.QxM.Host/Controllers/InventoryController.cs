using Microsoft.AspNetCore.Mvc;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Implementations;
using Qx.Application.Services.Interfaces;
using Qx.Application.Services.Utilities;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Utilities;
using Qx.Infrastructure.Persistence.Entities;
using Qx.QxM.Host.Dtos;
using ConsumableColumnDto = Qx.QxM.Host.Dtos.ConsumableColumnDto;
using DomainMappingUtility = Qx.Infrastructure.Persistence.Utilities.DomainMappingUtility;

namespace Qx.QxM.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryAppService _inventoryAppService;
        private readonly IConsumableTypeAppService _consumableTypeAppService;
        private readonly ILocationAppService _locationAppService;

        public InventoryController(IInventoryAppService inventoryAppService, 
            IConsumableTypeAppService consumableTypeAppService, ILocationAppService locationAppService)
        {
            _inventoryAppService = inventoryAppService;
            _consumableTypeAppService = consumableTypeAppService;
            _locationAppService = locationAppService;
        }

        /// <summary>
        /// Get the inventory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<InventoryDto>> GetInventoryAsync()
        {
            var inventory = await _inventoryAppService.GetInventoryAsync();
            return Ok(inventory);
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> AddConsumableAsync([FromForm] Guid consumableTypeId, [FromForm] Guid locationId, 
        //    [FromForm] ReusePolicyDto? reusePolicyDto, [FromForm] SealPolicyDto sealPolicyDto, 
        //    [FromForm] List<double> capacities)
        //{
        //    var consumableType = await _consumableTypeAppService.GetConsumableTypeByIdAsync(consumableTypeId);
        //    if (capacities.Count != consumableType.ColumnCount) 
        //        throw new ArgumentException($"The number of capacities ({capacities.Count}) doesn't match the number of columns ({consumableType.ColumnCount}))");
        //    var location = await _locationAppService.GetLocationByIdAsync(locationId);
        //    var reusePolicy = reusePolicyDto != null
        //        ? new ReusePolicyDto
        //        {
        //            IsReusable = reusePolicyDto.IsReusable,
        //            MaxUses = reusePolicyDto.MaxUses,
        //        }
        //        : new ReusePolicyDto(
        //        {
        //            IsReusable = consumableType.DefaultIsReusable,
        //            MaxUses = consumableType.DefaultMaxUses,
        //        };
        //    // setup consumable column dto (Qx.Application.Services.Dtos)
        //    var consumableColumns = new List<ConsumableColumnDto>();
        //    // setup consumable dto (Qx.Application.Services.Dtos)
        //    var consumable = new ConsumableDto
        //    {
        //        
        //    }
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> AddConsumableAsync([FromForm] CreateConsumableDto createConsumableDto)
        {
            //var consumableColumns
            //var domainConsumable = DomainMappingUtility.MapToDomain(consumableDto);
            //await _inventoryAppService.AddConsumableAsync(domainConsumable);
            return Ok();
        }
    }
}
