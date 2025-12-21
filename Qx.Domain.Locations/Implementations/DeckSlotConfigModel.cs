using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

/// <summary>
/// Deck slot config model for validating the expected structure of a deck slot config
/// </summary>
public sealed class DeckSlotConfigModel 
    : Dictionary<DeckSlotNames, Dictionary<BatchNames, CoordinatePosition>>
{
    
}