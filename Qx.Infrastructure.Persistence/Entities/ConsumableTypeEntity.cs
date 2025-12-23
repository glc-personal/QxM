namespace Qx.Infrastructure.Persistence.Entities;

public class ConsumableTypeEntity
{
    public string Id { get; set; } = null!; // PK, matches ConsumableEntity.TypeId
    public string Name { get; set; } = null!;
    public string Version { get; set; } = null!;
    
    // Geometry
    public int Rows { get; set; }
    public int Columns { get; set; }
    
    // Full Geometry 
    public string GeometryJson { get; set; }
    
    // Default reuse policy (can be overwritten per instance if needed)
    public bool DefaultIsReusable { get; set; }
    public int? DefaultMaxUses { get; set; }
}