using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Dtos;

/// <summary>
/// Inventory of consumables on the instrument at a specific time
/// </summary>
public sealed class InventoryDto
{
    /// <summary>
    /// Unique identifier for the inventory
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Time the inventory was captured at
    /// </summary>
    public DateTime CapturedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Time the inventory was updated at
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// List of consumables in the inventory
    /// </summary>
    public IReadOnlyList<IConsumable> Consumables { get; init; } = new List<IConsumable>();
}