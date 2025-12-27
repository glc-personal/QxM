using Microsoft.Extensions.Logging;
using Qx.Application.Services.Dtos;
using Qx.Application.Services.Interfaces;
using Qx.Application.Services.Utilities;
using Qx.Domain.Consumables.Enums;
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
    
    public async Task<IList<ConsumableTypeDto>> GetConsumableTypesAsync(ConsumableTypes? type, CancellationToken cancellationToken = default)
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
            if (type != null && consumableType.Type != type)
                continue;
            var consumableTypeDto = DtoMappingUtility.MapToDto(consumableType);
            consumableTypesDto.Add(consumableTypeDto);
        }
        return consumableTypesDto;
    }

    public async Task<ConsumableTypeDto> GetConsumableTypeByIdAsync(Guid typeId, CancellationToken cancellationToken = default)
    {
        var domainTypes = await _repo.GetConsumableTypesAsync(cancellationToken);
        var domainType = domainTypes.FirstOrDefault(t => t.UniqueIdentifier == typeId);
        if (domainType == null) throw new KeyNotFoundException($"Consumable type with id {typeId} not found.");
        var consumableTypeDto = DtoMappingUtility.MapToDto(domainType);
        return consumableTypeDto;
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
        try
        {
            await _repo.RemoveConsumableTypeAsync(consumableId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to delete consumable type with provided id ({consumableId})", ex.Message);
        }
    }
}