namespace Qx.Infrastructure.Persistence.Entities;

public class ConsumableTypeEntity
{
    public string Id { get; set; } = null!; // PK, matches ConsumableEntity.TypeId
    public string Name { get; set; } = null!;
    public string Version { get; set; }

    // Geometry
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public double ColumnOffsetMm { get; set; }
    public double RowOffsetMm { get; set; }
    public double LengthMm { get; set; }
    public double WidthMm { get; set; }
    public double HeightMm { get; set; }
    public double FirstColumnOffsetMm { get; set; }
    public double HeightOffDeckMm { get; set; }
    
    // Default reuse policy (can be overwritten per instance if needed)
    public bool DefaultIsReusable { get; set; }
    public int? DefaultMaxUses { get; set; }
}