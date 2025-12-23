namespace Qx.Application.Services.Dtos;

/// <summary>
/// Consumable Type Data Type Object indirectly mapped to Qx.Domain.Consumables.IConsumableType
/// </summary>
public sealed class ConsumableTypeDto
{
    /// <summary>
    /// Unique identifier for the consumable type
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Name of the consumable type (mapped to the Qx.Domain.Consumables.Enums.ConsumableTypes)
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Consumable version string
    /// </summary>
    public string Version { get; init; }
    
    /// <summary>
    /// Number of rows in the consumable
    /// </summary>
    public int RowCount { get; init; }
    
    /// <summary>
    /// Number of columns in the consumable
    /// </summary>
    public int ColumnCount { get; init; }
    
    /// <summary>
    /// Geometry meta data of the consumable
    /// </summary>
    public string Geometry { get; init; }
    
    /// <summary>
    /// Default is reusable value for the consumable
    /// </summary>
    public bool DefaultIsReusable { get; init; }
    
    /// <summary>
    /// Default max uses value for the consumable (null = unlimited)
    /// </summary>
    public int? DefaultMaxUses { get; init; }
}