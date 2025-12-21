using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Dtos;

/// <summary>
/// Inventory of consumables on the instrument at a specific time
/// </summary>
public sealed class InventoryDto
{
    /// <summary>
    /// Time the inventory was captured at
    /// </summary>
    public DateTime CapturedAt { get; init; } = DateTime.UtcNow;
    
    // Consumables present
    // TODO: Cartridges, Lids
    public IReadOnlyList<IPlate> Plates { get; init; } = new List<IPlate>();
    public IReadOnlyList<ITipBox> TipBoxes { get; init; } = new List<ITipBox>();
}