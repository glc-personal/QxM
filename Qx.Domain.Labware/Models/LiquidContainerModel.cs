using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Labware.Models;

public sealed class LiquidContainerModel
{
    internal LiquidContainerModel(LabwareGrid grid, IReadOnlyList<WellColumnDefinition> columnDefinitions)
    {
        RowCount = grid.RowCount;
        ColumnCount = grid.ColumnCount;
        Columns = columnDefinitions;
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public IReadOnlyList<WellColumnDefinition> Columns { get; }

    public static LiquidContainerModel Create(LabwareGrid grid, IReadOnlyList<WellColumnDefinition> columnDefinitions)
    {
        EnforceDomainInvariants(grid, columnDefinitions);
        return new LiquidContainerModel(grid, columnDefinitions);
    }

    private static void EnforceDomainInvariants(LabwareGrid grid, IReadOnlyList<WellColumnDefinition> columnDefinitions)
    {
        // ensure domain invariant: number of columns must be greater than 0
        if (grid.ColumnCount <= 0)
            throw new ArgumentException("Grid must have at least one column", nameof(grid));
        // enforce domain invariant: grid column count must match number of column definitions provided
        if (grid.ColumnCount != columnDefinitions.Count)
            throw new ArgumentException($"{nameof(LabwareGrid)} ({grid.ColumnCount}) " +
                                        $"and {nameof(IReadOnlyList<WellColumnDefinition>)} ({columnDefinitions.Count}) " +
                                        $"must have the same number of columns.");
        // enforce domain invariant: ensure all column definitions have unique column indexes
        var sorted = columnDefinitions.ToList();
        sorted.Sort((a, b) => a.ColumnIndex.CompareTo(b.ColumnIndex));
        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i].ColumnIndex == sorted[i - 1].ColumnIndex)
                throw new ArgumentException($"Duplicate column definition for ColumnIndex {sorted[i].ColumnIndex}.", nameof(columnDefinitions));
        }
        // enforce domain invariant: ensure column indexes are continuously ordered (e.g. 1,2,5,6 is bad),
        //                           well definitions have valid capacities and identical well shapes and well bottoms
        var expectedWellShape = sorted[0].WellDefinition.Shape;
        var expectedWellBottom = sorted[0].WellDefinition.Bottom;
        for (int expectedIndex = 0; expectedIndex < sorted.Count; expectedIndex++)
        {
            var columnDefinition = sorted[expectedIndex];
            if (columnDefinition.ColumnIndex != expectedIndex)
                throw new ArgumentException($"Missing or mis-indexed  column definition. Expected {expectedIndex} but found {columnDefinition.ColumnIndex}.", nameof(columnDefinitions));
            if (columnDefinition.WellDefinition.Capacity.MaxVolume.Value <= 0)
                throw new ArgumentException($"Columns (index: {columnDefinition.ColumnIndex}) of wells must have positive capacities ({columnDefinition.WellDefinition.Capacity.MaxVolume.ToString()})");
            if (columnDefinition.WellDefinition.Shape != expectedWellShape)
                throw new ArgumentException($"Column (index: {columnDefinition.ColumnIndex}) has a well shape: {columnDefinition.WellDefinition.Shape} but expected {expectedWellShape}");
            if (columnDefinition.WellDefinition.Bottom != expectedWellBottom)
                throw new ArgumentException($"Column (index: {columnDefinition.ColumnIndex}) has a well bottom: {columnDefinition.WellDefinition.Bottom} but expected {expectedWellBottom}");
        }
    }
}