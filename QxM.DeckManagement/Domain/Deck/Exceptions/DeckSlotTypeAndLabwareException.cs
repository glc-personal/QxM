using Qx.Domain.Labware.LabwareDefinitions;

namespace QxM.DeckManagement.Domain.Deck.Exceptions;

public class DeckSlotTypeAndLabwareException(DeckSlotType deckSlotType, LabwareDefinitionReference labware)
    : ArgumentException($"Expected {deckSlotType} labware but got {labware.Name}");