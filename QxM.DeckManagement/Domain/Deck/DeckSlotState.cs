using Qx.Domain.Labware.LabwareDefinitions;

namespace QxM.DeckManagement.Domain.Deck;

// TODO: replace LabwareDefinitionReference with a Labware instance (we need the instance not the definition), think if this is the case or not??
public readonly record struct DeckSlotState(LabwareDefinitionReference? Labware = null, Occupancy Occupancy = Occupancy.Unoccupied)
{
    public static DeckSlotState Occupied(LabwareDefinitionReference? labware) => new(labware, Occupancy.Occupied);
    public static DeckSlotState Unoccupied => new(null, Occupancy.Unoccupied);
}