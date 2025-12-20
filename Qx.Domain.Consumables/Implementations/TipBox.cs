using Qx.Core.Mathematics.Implementations;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

public class TipBox : ITipBox
{
    private IDictionary<int, TipColumn> _tipColumns;
    private ConsumableStates _state;
    private int _uses;
    private Range<int> _rowRange;
    private Range<int> _columnRange;
    private ReusePolicy _columnReusePolicy;

    /// <summary>
    /// Tip Box
    /// </summary>
    /// <param name="batch">Associate batch</param>
    /// <param name="columnCount">Number of columns</param>
    /// <param name="rowCount">Number of rows</param>
    /// <param name="columnResusePolicy">Reuse policy for the tip columns</param>
    public TipBox(BatchNames batch, int columnCount, int rowCount, ReusePolicy columnResusePolicy)
    {
        InitializeTipBoxColumns(columnCount, rowCount);
        Name = ConsumableNamingUtility.CreateConsumableName(DeckSlotNames.TipBox, batch);
        UniqueIdentifier = Guid.NewGuid();
        Type = ConsumableTypes.TipBox;
        _state = ConsumableStates.Available; // available if any column has uses left (consumed if all columns consumed)
        ReusePolicy = new ReusePolicy(true);
        _uses = 0;
        Location = new Location(Name, new DeckSlotPosition(DeckSlotNames.TipBox, batch));
        _rowRange = new Range<int>(0, rowCount, true, false);
        _columnRange = new Range<int>(0, columnCount, true, false);
        _columnReusePolicy = columnResusePolicy;
        NumberOfColumns = columnCount;
        NumberOfRows = rowCount;
    }
    
    public string Name { get; }
    public Guid UniqueIdentifier { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State => _state;
    public ReusePolicy ReusePolicy { get; }
    public int Uses => _uses;
    public Location Location { get; }
    public IReadOnlyDictionary<int, TipColumn> TipColumns => (IReadOnlyDictionary<int, TipColumn>)_tipColumns;

    public int NumberOfColumns { get; }
    public int NumberOfRows { get; }

    /// <inheritdoc/> ...
    public void AddNewTips(ITip tip, int columnIndex)
    {
        if (_tipColumns.ContainsKey(columnIndex))
            throw new InvalidOperationException($"Invalid operation, this column ({Name}-{_tipColumns[columnIndex].Name}) already contains tips.");
        _tipColumns[columnIndex].AddNewTips(tip);
    }

    public void AddNewTips(ITip tip, IList<int> columnIndexes, bool failOnOne = true)
    {
        var failedColumnIndexes = new List<int>();
        foreach (var columnIndex in columnIndexes)
        {
            if (_tipColumns.ContainsKey(columnIndex) && _tipColumns[columnIndex].ContainsTips())
            {
                if (failOnOne)
                    throw new InvalidOperationException($"Invalid operation, this column ({Name}-{_tipColumns[columnIndex].Name}) already contains tips.");
                failedColumnIndexes.Add(columnIndex);
            }
            _tipColumns[columnIndex].AddNewTips(tip);
        }

        if (failedColumnIndexes.Count > 0)
        {
            var failures = string.Join(", ", failedColumnIndexes);
            throw new InvalidOperationException($"Invalid operation, this column ({Name}-Column(s)[{failures}]) already contains tips.");
        }
    }

    /// <summary>
    /// Add tips to a column in the tip box
    /// </summary>
    /// <param name="tips">Tips to be added to a column in the tip box</param>
    /// <param name="columnIndex">Column index where tips will be added</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddTips(IList<ITip> tips, int columnIndex)
    {
        if (_tipColumns[columnIndex].ContainsTips())
            throw new InvalidOperationException($"Invalid operation, this column ({Name}-{_tipColumns[columnIndex].Name}) already contains tips.");
        _tipColumns[columnIndex].AddTips(tips);
    }

    public IList<ITip> RemoveTips(int columnIndex)
    {
        if (_tipColumns[columnIndex].IsEmpty())
            throw new InvalidOperationException($"Invalid operation, this column ({Name}-{_tipColumns[columnIndex].Name}) already contains tips.");
        _uses++;
        return _tipColumns[columnIndex].RemoveTips();
    }

    /// <summary>
    /// Initialize the tip box columns for the tip box
    /// </summary>
    /// <param name="columnCount">Number of columns in the tip box (number of tip columns)</param>
    /// <param name="rowCount">Number of rows in the tip box (number of tips)</param>
    private void InitializeTipBoxColumns(int columnCount, int rowCount)
    {
        _tipColumns = new Dictionary<int, TipColumn>();
        for (int i = 0; i < columnCount; i++)
            _tipColumns[i] = new TipColumn(i, rowCount, new ReusePolicy(true));
    }
}