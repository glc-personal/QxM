using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

/// <summary>
/// Deck slot position to specify where on the work-deck a position is
/// </summary>
public sealed record DeckSlotPosition : Position
{
    public DeckSlotPosition(SlotName name, BatchNames batch)
    {
        Name = name;
        Batch = batch;
    }
    
    public SlotName Name { get; }
    public BatchNames Batch { get; }
}