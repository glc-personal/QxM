using Qx.Domain.Labware.LabwareDefinitions;
using QxM.DeckManagement.Domain.Deck.Exceptions;

namespace QxM.DeckManagement.Domain.Deck;

public sealed class DeckSlot
{
    private DeckSlotState _state;

    private DeckSlot(DeckSlotDefinition definition)
    {
        Key = definition.Key;
        Pose = definition.Pose;
        _state = DeckSlotState.Unoccupied;
    }
    
    public DeckSlotKey Key { get; }
    public DeckSlotPose Pose { get; }
    public DeckSlotState State => _state;

    /// <summary>
    /// Build a deck slot from a definition
    /// </summary>
    /// <param name="definition"></param>
    /// <returns></returns>
    public static DeckSlot Create(DeckSlotDefinition definition)
    {
        return new DeckSlot(definition);
    }

    /// <summary>
    /// Add labware to the deck slot
    /// </summary>
    /// <param name="labware"></param>
    public void AddLabware(LabwareDefinitionReference labware)
    {
        EnforceSingleOccupancy();
        EnforceValidOccupancy(labware);
        _state = DeckSlotState.Occupied(labware);
    }

    /// <summary>
    /// Remove labware from the deck slot
    /// </summary>
    public void RemoveLabware() 
        => _state = DeckSlotState.Unoccupied;

    /// <summary>
    /// Enforce single occupancy for a deck slot domain invariant
    /// </summary>
    /// <exception cref="DeckSlotAlreadyOccupiedException"></exception>
    private void EnforceSingleOccupancy()
    {
        if (_state.Occupancy == Occupancy.Occupied)
            throw new DeckSlotAlreadyOccupiedException(Key);
    }

    /// <summary>
    /// Enforce labware definition name matches the deck slot key type (only allow certain labware in certain slots)
    /// </summary>
    /// <param name="labware"></param>
    /// <exception cref="DeckSlotTypeAndLabwareException"></exception>
    private void EnforceValidOccupancy(LabwareDefinitionReference labware)
    {
        if (Key.Type.ToString() != labware.Name)
            throw new DeckSlotTypeAndLabwareException(Key.Type, labware);
    }
}