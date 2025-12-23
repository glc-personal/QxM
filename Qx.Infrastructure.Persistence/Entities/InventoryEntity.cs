namespace Qx.Infrastructure.Persistence.Entities;

public class InventoryEntity
{
    public Guid Id { get; set; } // PK
    
    public DateTime CreatedAtDate { get; set; }
    public DateTime UpdatedAtDate { get; set; }
    
    // Foreign Relationships
    public ICollection<ConsumableEntity> Consumables { get; set; } 
        = new List<ConsumableEntity>();
    
    // Row version for optimistic concurrency control (OCC)
    public byte[] RowVersion { get; set; } = [];
}