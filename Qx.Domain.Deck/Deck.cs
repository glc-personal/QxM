using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Deck;

public sealed class Deck
{
    private IReadOnlyDictionary<DeckSlotKey, DeckSlot> _slots;
    
    private Deck(DeckDefinition definition, Dictionary<DeckSlotKey, DeckSlot> slots)
    {
        Geometry = definition.Geometry;
        _slots = slots;
    }

    public DeckGeometry Geometry { get; }

    /// <summary>
    /// Create a deck instance
    /// </summary>
    /// <param name="definition"></param>
    /// <returns></returns>
    public static Deck Create(DeckDefinition definition)
    {
        var slots = new Dictionary<DeckSlotKey, DeckSlot>();
        foreach (var slotDefinition in definition.SlotDefinitions)
            slots.Add(slotDefinition.Key, DeckSlot.Create(slotDefinition));
        return new Deck(definition, slots);
    }

    public void AddLabware(LabwareDefinitionReference labware, DeckSlotKey slotKey)
        => _slots[slotKey].AddLabware(labware);
    
    public void RemoveLabware(DeckSlotKey slotKey) 
        => _slots[slotKey].RemoveLabware();
    
    public DeckSlotState GetDeckSlotState(DeckSlotKey slotKey)
    => _slots[slotKey].State;
}