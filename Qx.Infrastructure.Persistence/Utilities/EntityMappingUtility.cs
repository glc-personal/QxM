using Qx.Domain.Consumables.Interfaces;
using Qx.Infrastructure.Persistence.Entities;

namespace Qx.Infrastructure.Persistence.Utilities;

public static class EntityMappingUtility
{
    public static InventoryEntity MapToEntity(IInventory inventory)
    {
        var entity = InitializeInventoryEntity();

        //foreach (var domainConsumable in inventory.Consumables)
        //{
        //    var consumableEntity = 
        //}
        throw new NotImplementedException();
    }

    public static ConsumableEntity MapToEntity(IConsumable consumable, IInventory inventory)
    {
        var entity = InitializeConsumableEntity();
        throw new NotImplementedException();
    }

    public static ConsumableTypeEntity MapToEntity(IConsumableType consumableType)
    {
        var entity = new ConsumableTypeEntity
        {
            Id = consumableType.UniqueIdentifier.ToString(),
            Name = consumableType.Name,
            Version = consumableType.Version.ToString(),
            Rows = consumableType.RowCount,
            Columns = consumableType.ColumnCount,
            GeometryJson = consumableType.Geometry,
            DefaultIsReusable = consumableType.DefaultIsReusable,
            DefaultMaxUses = consumableType.DefaultMaxUses
        };
        return entity;
    }

    private static InventoryEntity InitializeInventoryEntity()
    {
        var now = DateTime.UtcNow;
        
        var entity = new InventoryEntity
        {
            Id = Guid.NewGuid(),
            CreatedAtDate = now,
            UpdatedAtDate = now,
            Consumables = new List<ConsumableEntity>()
        };
        return entity;
    }

    private static ConsumableEntity InitializeConsumableEntity()
    {
        var entity = new ConsumableEntity
        {
            Id = Guid.NewGuid(),
            Barcode = null,
            Columns = new List<ConsumableColumnEntity>(),
            Inventory = InitializeInventoryEntity(),
            InventoryId = Guid.NewGuid(),
            IsReusable = true,
            Location = null,
            LocationId = Guid.NewGuid(),
            MaxUses = null,
            Name = null
        };
        return entity;
    }
}