using Microsoft.Extensions.Logging;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Application.Services.Utilities;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Implementations;

public sealed class ConsumableTypeAppService : IConsumableTypeAppService
{
    private readonly IConsumableTypeRepository _repo;
    private readonly ILogger<ConsumableTypeAppService> _logger;

    public ConsumableTypeAppService(IConsumableTypeRepository repo, ILogger<ConsumableTypeAppService> logger)
    {
        _repo = repo;
        _logger = logger;
    }
    
    public async Task<IList<ConsumableTypeDto>> GetConsumableTypesAsync(CancellationToken cancellationToken = default)
    {
        var consumableTypes = await _repo.GetConsumableTypesAsync(cancellationToken);

        if (consumableTypes is null)
        {
            _logger.LogInformation("No consumable types found. Returning empty list.");
            return Array.Empty<ConsumableTypeDto>();
        }
        
        IList<ConsumableTypeDto> consumableTypesDto = new List<ConsumableTypeDto>();
        foreach (var consumableType in consumableTypes)
        {
            var consumableTypeDto = DtoMappingUtility.MapToDto(consumableType);
            consumableTypesDto.Add(consumableTypeDto);
        }
        return consumableTypesDto;
    }

    public async Task AddConsumableTypeAsync(ConsumableTypeDto consumableTypeDto, CancellationToken cancellationToken = default)
    {
        await _repo.AddConsumableTypeAsync(DomainMappingUtility.MapToDomain(consumableTypeDto), cancellationToken);
    }

    public Task UpdateConsumableTypeAsync(Guid consumableTypeId, ConsumableTypeDto consumableTypeDto,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteConsumableTypeAsync(Guid consumableId, CancellationToken cancellationToken = default)
    {
        await _repo.RemoveConsumableTypeAsync(consumableId, cancellationToken);
    }
}