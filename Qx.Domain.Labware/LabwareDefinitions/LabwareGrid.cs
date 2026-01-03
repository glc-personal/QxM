using Qx.Core.Measurement;

namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Labware grid definition for specifying the rows and columns as a grid
/// </summary>
public sealed record LabwareGrid
{
    /// <summary>
    /// Labware grid definition for specifying the rows and columns as a grid
    /// <param name="rowCount">Number of rows</param>
    /// <param name="columnCount">Number of columns</param>
    /// <param name="rowPitchMm">Spacing between rows in millimeters</param>
    /// <param name="columnPitchMm">Spacing between columns in millimeters</param>
    /// <param name="firstColumnOffsetMm">Spacing between the start of the labware and the first column</param>
    /// </summary>
    public LabwareGrid(int rowCount, int columnCount,
        Mm rowPitchMm, Mm columnPitchMm, Mm firstColumnOffsetMm)
    {
        EnsureRowAndColumnCountIsNonZeroAndUnsigned(rowCount, columnCount);
        RowCount = rowCount;
        ColumnCount = columnCount;
        EnsureDimensionsAreNonZero(rowPitchMm, columnPitchMm, firstColumnOffsetMm);
        RowPitchMm = rowPitchMm;
        ColumnPitchMm = columnPitchMm;
        FirstColumnOffsetMm = firstColumnOffsetMm;
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public Mm RowPitchMm { get; }
    public Mm ColumnPitchMm { get; }
    public Mm FirstColumnOffsetMm { get; }

    private void EnsureRowAndColumnCountIsNonZeroAndUnsigned(int rowCount, int columnCount)
    {
        if (rowCount <= 0 || columnCount <= 0)
            throw new ArgumentOutOfRangeException($"The number of rows ({rowCount}) and columns ({columnCount}) must be non-negative and greater than 0");
    }
    
    
    private void EnsureDimensionsAreNonZero(Mm rowPitchMm, Mm columnPitchMm,
        Mm firstColumnOffsetMm)
    {
        if (rowPitchMm.Equals(Mm.Zero) || columnPitchMm.Equals(Mm.Zero) || firstColumnOffsetMm.Equals(Mm.Zero))
            throw new ArgumentOutOfRangeException($"{nameof(LabwareGrid)} requirues non-zero dimensions " +
                                                  $"({nameof(RowPitchMm)}: {rowPitchMm}, " +
                                                  $"{nameof(ColumnPitchMm)}: {columnPitchMm}, " +
                                                  $"{nameof(FirstColumnOffsetMm)}: {firstColumnOffsetMm})");
    }
}