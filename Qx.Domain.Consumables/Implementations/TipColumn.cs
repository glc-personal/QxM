using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Column of tips
/// </summary>
public sealed class TipColumn : IConsumable
{
    private IList<Tip> _tips;
    private int _uses;
    private ConsumableStates _state;
    
    public TipColumn(int columnIndex, ReusePolicy reusePolicy)
    {
        Name = ConsumableNamingUtility.CreateColumnName(columnIndex);
        UniqueIdentifier = Guid.NewGuid();
        Type = ConsumableTypes.TipColumn;
        _state = ConsumableStates.Available; // available because it is free for tips (in use means it has tips in it)
        ReusePolicy = reusePolicy;
        _uses = 0;
        _tips = new List<Tip>();
        Location = new Location(Name, new ColumnPosition(columnIndex));
        ColumnIndex = columnIndex;
    }
    
    public int ColumnIndex { get; }
    public int TipCount => _tips.Count;

    /// <summary>
    /// Add tips to the tip column
    /// </summary>
    /// <param name="tips">Tips to be added to the column</param>
    public void AddTips(IList<Tip> tips)
    {
        if (_state == ConsumableStates.InUse)
            throw new InvalidOperationException($"Cannot add tips to this column ({Name}), it is already in use.");
        _tips = tips;
        _state = ConsumableStates.InUse;
    }

    /// <summary>
    /// Remove the tips from the tip column
    /// </summary>
    /// <exception cref="InvalidOperationException">Exception if tips are removed when there are no tips</exception>
    /// <returns>the list of tips</returns>
    public IList<Tip> RemoveTips()
    {
        if (_state == ConsumableStates.Available)
            throw new InvalidOperationException($"Cannot remove tips, there are no tips to remove");
        if (!ReusePolicy.CanUse(_uses))
            throw new OutOfUsesException(_uses, ReusePolicy.MaxUses.Value);
        
        var tips = _tips;
        _tips = new List<Tip>();
        _uses += 1;
        _state = _uses > ReusePolicy.MaxUses ? ConsumableStates.Consumed : ConsumableStates.Available;
        return tips;
    }

    public string Name { get; }
    public Guid UniqueIdentifier { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State => _state;
    public ReusePolicy ReusePolicy { get; }
    public int Uses => _uses;
    public Location Location { get; }
}