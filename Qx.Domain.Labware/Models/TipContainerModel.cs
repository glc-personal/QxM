using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Labware.Models;

public sealed class TipContainerModel
{
    private TipContainerModel(LabwareGrid grid, IReadOnlyList<TipColumnDefinition> columnDefinitions)
    {
        RowCount = grid.RowCount;
        ColumnCount = grid.ColumnCount;
        Columns = columnDefinitions;
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public IReadOnlyList<TipColumnDefinition> Columns { get; }

    public static TipContainerModel Create(LabwareGrid grid, IReadOnlyList<TipColumnDefinition> columnDefinitions)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(columnDefinitions);
        EnforceDomainInvariants(grid, columnDefinitions);
        return new TipContainerModel(grid, columnDefinitions);
    }
    
    // NOTE: allowing missing columns in the tip container model to model tip boxes with missing tip columns
    private static void EnforceDomainInvariants(LabwareGrid grid, IReadOnlyList<TipColumnDefinition> columnDefinitions)
    {
        // ensure domain invariant: number of columns must be greater than 0
        if (grid.ColumnCount <= 0)
            throw new ArgumentException("Grid must have at least one column", nameof(grid));
        // ensure domain invariant: number of columns in the labware grid match the number of tip column definitions provided
        if (grid.ColumnCount != columnDefinitions.Count)
            throw new ArgumentException($"{nameof(LabwareGrid)} and {nameof(IReadOnlyList<TipColumnDefinition>)} must have the same number of columns");
        // enforce domain invariant: ensure all column definitions have unique column indexes
        var sorted = columnDefinitions.ToList();
        sorted.Sort((a, b) => a.ColumnIndex.CompareTo(b.ColumnIndex));
        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i].ColumnIndex == sorted[i - 1].ColumnIndex)
                throw new ArgumentException($"Duplicate column definition for ColumnIndex {sorted[i].ColumnIndex}.", nameof(columnDefinitions));
        }
        // ensure domain invariant: can't have a column index larger than the number of columns in the grid
        if (sorted[^1].ColumnIndex > sorted.Count - 1)
            throw new ArgumentException($"Cannot have a column index ({sorted[^1].ColumnIndex}) larger than the number of columns in the grid");
    }
}