namespace Qx.Domain.Consumables.Implementations;

public sealed class ConsumableGeometry
{
    /// <summary>
    /// Number of rows (number of tips/wells)
    /// </summary>
    public int RowCount { get; set; }

    /// <summary>
    /// Number of columns
    /// </summary>
    public int ColumnCount { get; set; }

    /// <summary>
    /// Space between columns in millimeters
    /// </summary>
    public double ColumnOffsetMm { get; set; }

    /// <summary>
    /// Space between rows in millimeters
    /// </summary>
    public double RowOffsetMm { get; set; }

    /// <summary>
    /// Length of the consumable in millimeters (column-wise)
    /// </summary>
    public double LengthMm { get; set; }

    /// <summary>
    /// Width of the consumable in millimeters (row-wise)
    /// </summary>
    public double WidthMm { get; set; }

    /// <summary>
    /// Height of the consumable in millimeters
    /// </summary>
    public double HeightMm { get; set; }

    /// <summary>
    /// Space between the edge of the consumable and the first column in millimeters
    /// </summary>
    public double FirstColumnOffsetMm { get; set; }

    /// <summary>
    /// Height from top of consumable to the deck in millimeters
    /// </summary>
    public double HeightOffDeckMm { get; set; }
}