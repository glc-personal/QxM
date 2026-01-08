namespace QxM.DeckManagement.Domain.Deck;

/// <summary>
/// Deck slot value object representing a slot on the deck for labware
/// </summary>
/// <param name="Key">Deck slot key</param>
/// <param name="Pose">Deck slot pose relative the deck's origin</param>
public readonly record struct DeckSlotDefinition(DeckSlotKey Key, DeckSlotPose Pose);
