namespace Qx.Infrastructure.Persistence.Entities;

public class ConsumableEntity
{
    public Guid Id { get; set; } // PK
    public Guid InventoryId { get; set; } // FK -> maps to InventoryEntity.Id
    public InventoryEntity Inventory { get; set; } = null!; // navigation
    public string Name { get; set; } = null!; // friendly name
    public string TypeId { get; set; } = null!; // maps to ConsumableTypeId in domain
    public ConsumableTypeEntity? Type { get; set; }
    public int State { get; set; } // maps to domain ConsumableState enum
    public bool IsReusable { get; set; } // pulled from ReusePolicy
    public int? MaxUses { get; set; } // null = unlimited
    public int Uses { get; set; } // number of uses
    public string? Barcode { get; set; }
    public Guid LocationId { get; set; } // FK -> LocationEntity.Id
    public LocationEntity? Location { get; set; } // navigation
    public IList<ConsumableColumnEntity> Columns { get; set; } 
        = new List<ConsumableColumnEntity>();
}