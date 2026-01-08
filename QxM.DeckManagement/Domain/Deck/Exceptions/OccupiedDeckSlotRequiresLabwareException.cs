using Qx.Domain.Labware.LabwareDefinitions;

namespace QxM.DeckManagement.Domain.Deck.Exceptions;

public class OccupiedDeckSlotRequiresLabwareException(DeckSlotKey slotKey)
    : InvalidOperationException($"{slotKey.Label} requires {nameof(LabwareId)} to be {Occupancy.Occupied.ToString().ToLower()}");