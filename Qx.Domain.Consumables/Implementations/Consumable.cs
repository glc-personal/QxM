using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

public class Consumable : IConsumable
{
    private ConsumableStates _state;
    private int _uses;
    private IList<ConsumableColumn> _columns;

    public Consumable(DeckSlotNames deckSlot, BatchNames batch, ConsumableTypes type,
        int numberOfColumns, int numberOfRows, 
        ReusePolicy reusePolicy, SealPolicy sealPolicy)
    {
        Name = ConsumableNamingUtility.CreateConsumableName(deckSlot, batch);
        UniqueIdentifier = Guid.NewGuid();
        Type = type;
        _state = ConsumableStates.Available;
        ReusePolicy = reusePolicy;
        _uses = 0;
        Location = new Location(Name, new DeckSlotPosition(deckSlot, batch));
        NumberOfColumns = numberOfColumns;
        NumberOfRows = numberOfRows;
        _columns = new List<ConsumableColumn>(numberOfColumns);
    }
    
    public string Name { get; }
    public Guid UniqueIdentifier { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State => _state;
    public ReusePolicy ReusePolicy { get; }
    public int Uses => _uses;
    public Location Location { get; }
    public int NumberOfColumns { get; }
    public int NumberOfRows { get; }
    public IList<ConsumableColumn> Columns => _columns;
    
    public void OverrideUses(int value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException($"Cannot override {nameof(Consumable)}.{nameof(Uses)} to a negative value.");
        _uses = value;
        if (ReusePolicy.IsReusable && _uses > ReusePolicy.MaxUses.Value)
            _state = ConsumableStates.Consumed;
    }

    public void OverrideState(ConsumableStates value)
    {
        if (!ReusePolicy.IsReusable && value == ConsumableStates.Consumed) 
            throw new ArgumentException($"Cannot override {nameof(Consumable)}.{nameof(State)} to a non-reusable value.");
        _state = value;
        if (ReusePolicy.IsReusable && _uses < ReusePolicy.MaxUses.Value && value == ConsumableStates.Consumed)
            _uses = ReusePolicy.MaxUses.Value + 1;
    }
}