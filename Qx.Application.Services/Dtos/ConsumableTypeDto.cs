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
    /// Spacing between columns in millimeters
    /// </summary>
    public double ColumnOffsetMm { get; init; }
    
    /// <summary>
    /// Spacing between rows in millimeters
    /// </summary>
    public double RowOffsetMm { get; init; }
    
    /// <summary>
    /// Consumable length in millimeters
    /// </summary>
    public double LengthMm { get; init; }
    
    /// <summary>
    /// Consumable width in millimeters (row-wise)
    /// </summary>
    public double WidthMm { get; init; }
    
    /// <summary>
    /// Consumable height in millimeters
    /// </summary>
    public double HeightMm { get; init; }
    
    /// <summary>
    /// Distance to the first column from the beginning of the consumable in millimeters
    /// </summary>
    public double FirstColumnOffsetMm { get; init; }
    
    /// <summary>
    /// Height from the deck and the top of the consumable in millimeters
    /// </summary>
    public double HeightOffDeckMm { get; init; }
    
    /// <summary>
    /// Default is reusable value for the consumable
    /// </summary>
    public bool DefaultIsReusable { get; init; }
    
    /// <summary>
    /// Default max uses value for the consumable (null = unlimited)
    /// </summary>
    public int? DefaultMaxUses { get; init; }
}