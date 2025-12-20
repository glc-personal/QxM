using Qx.Core;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Column of tips
/// </summary>
public sealed class TipColumn : INameable
{
    private IList<Tip> _tips;
    
    public TipColumn(int columnIndex)
    {
        Name = ConsumableNamingUtility.CreateColumnName(columnIndex);
        _tips = new List<Tip>();
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
        _tips = tips;
    }

    /// <summary>
    /// Remove the tips from the tip column
    /// </summary>
    /// <exception cref="InvalidOperationException">Exception if tips are removed when there are no tips</exception>
    /// <returns>the list of tips</returns>
    public IList<Tip> RemoveTips()
    {
        if (_tips.Count == 0)
            throw new InvalidOperationException($"Cannot remove tips, there are no tips to remove");
        var tips = _tips;
        _tips = new List<Tip>();
        return tips;
    }

    public string Name { get; }
}