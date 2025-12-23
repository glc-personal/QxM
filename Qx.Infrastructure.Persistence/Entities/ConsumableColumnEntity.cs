namespace Qx.Infrastructure.Persistence.Entities;

public class ConsumableColumnEntity
{
    public Guid Id { get; set; } // PK
    public string Name { get; set; } = null!; // Column{columnIndex+1}
    
    // parent consumable
    public Guid ConsumableId { get; set; } // FK -> Consumable.Id
    public ConsumableEntity Consumable { get; set; } = null!; // navigation
    
    // Lists need special handling in EF (no direct mapping from a list to multiple columns)
    public IList<double> VolumeCapacityUl { get; set; } = new List<double>();
    public IList<double> CurrentVolumeUl { get; set; } = new List<double>();
    
    public bool IsReusable { get; set; }
    public int? MaxUses { get; set; }
    public int Uses { get; set; }
    public bool IsSealed { get; set; }
}