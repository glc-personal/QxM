using Qx.Domain.Consumables.Interfaces;
using Qx.Infrastructure.Persistence.Contexts;
using Qx.Infrastructure.Persistence.Utilities;

namespace Qx.Infrastructure.Persistence.Repositories;

public class ConsumableTypeRepository : IConsumableTypeRepository
{
    private readonly QxMDbContext _context;

    public ConsumableTypeRepository(QxMDbContext context)
    {
        _context = context;
    }
    
    public Task<IList<IConsumableType>> GetConsumableTypesAsync(CancellationToken cancellationToken = default)
    {
        var consumableTypes = new List<IConsumableType>();
        var consumableTypeEntites = _context.ConsumableType.ToList();
        foreach (var consumableTypeEntity in consumableTypeEntites)
        {
            var consumableType = DomainMappingUtility.MapToDomain(consumableTypeEntity);
            consumableTypes.Add(consumableType);
        }
        return Task.FromResult<IList<IConsumableType>>(consumableTypes);
    }

    public Task AddConsumableTypeAsync(IConsumableType consumableType, CancellationToken cancellationToken = default)
    {
        var consumableTypeEntity = EntityMappingUtility.MapToEntity(consumableType);
        _context.ConsumableType.Add(consumableTypeEntity);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task RemoveConsumableTypeAsync(Guid consumableTypeId, CancellationToken cancellationToken = default)
    {
        var entity = _context.ConsumableType.FirstOrDefault(e => e.Id == consumableTypeId.ToString());
        if (entity == null) throw new KeyNotFoundException($"Consumable type with id {consumableTypeId} not found");
        _context.ConsumableType.Remove(entity);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task UpdateConsumableTypeAsync(IConsumableType consumableType, CancellationToken cancellationToken = default)
    {
        var consumableTypeEntity = EntityMappingUtility.MapToEntity(consumableType);
        _context.ConsumableType.Update(consumableTypeEntity);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}