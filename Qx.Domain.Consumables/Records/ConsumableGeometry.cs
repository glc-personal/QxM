namespace Qx.Domain.Consumables.Records;

public sealed record ConsumableGeometry
{
    public ConsumableGeometry(int rowCount, int columnCount, double columnOffsetMm, double rowOffsetMm, double lengthMm,
        double widthMm, double heightMm, double firstColumnOffsetMm, double heightOffDeckMm)
    {
        RowCount = GetValue(rowCount);
        ColumnCount = GetValue(columnCount);
        ColumnOffsetMm = GetValue(columnOffsetMm);
        RowOffsetMm = GetValue(rowOffsetMm);
        LengthMm = GetValue(lengthMm);
        WidthMm = GetValue(widthMm);
        HeightMm = GetValue(heightMm);
        FirstColumnOffsetMm = GetValue(firstColumnOffsetMm);
        HeightOffDeckMm = GetValue(heightOffDeckMm);
    }
    
    /// <summary>
    /// Number of rows (number of tips/wells)
    /// </summary>
    public int RowCount { get; init; }

    /// <summary>
    /// Number of columns
    /// </summary>
    public int ColumnCount { get; init; }

    /// <summary>
    /// Space between columns in millimeters
    /// </summary>
    public double ColumnOffsetMm { get; init; }

    /// <summary>
    /// Space between rows in millimeters
    /// </summary>
    public double RowOffsetMm { get; init; }

    /// <summary>
    /// Length of the consumable in millimeters (column-wise)
    /// </summary>
    public double LengthMm { get; init; }

    /// <summary>
    /// Width of the consumable in millimeters (row-wise)
    /// </summary>
    public double WidthMm { get; init; }

    /// <summary>
    /// Height of the consumable in millimeters
    /// </summary>
    public double HeightMm { get; init; }

    /// <summary>
    /// Space between the edge of the consumable and the first column in millimeters
    /// </summary>
    public double FirstColumnOffsetMm { get; init; }

    /// <summary>
    /// Height from top of consumable to the deck in millimeters
    /// </summary>
    public double HeightOffDeckMm { get; init; }
    
    private static int GetValue(int value) => value < 0 
        ? throw new ArgumentException($"{nameof(ConsumableGeometry)} {nameof(value)} cannot be negative") 
        : value;
    private static double GetValue(double value) => value < 0 
        ? throw new ArgumentException($"{nameof(ConsumableGeometry)} {nameof(value)} cannot be negative") 
        : value;
}