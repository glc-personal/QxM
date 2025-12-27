using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public sealed class ConsumableGeometryDto
{
    [Range(8, 8)]
    [SwaggerSchema(Description = "Number of rows")]
    public int RowCount { get; set; }
    [Range(1, 12)]
    [SwaggerSchema(Description = "Number of columns")]
    public int ColumnCount { get; set; }
    [Range(0, 100.0D)]
    [SwaggerSchema(Description = "Spacing between columns in millimeters")]
    public double ColumnOffsetMm { get; set; }
    [Range(0, 100.0D)]
    [SwaggerSchema(Description = "Spacing between rows in millimeters")]
    public double RowOffsetMm { get; set; }
    [Range(10.0, 300.0D)]
    [SwaggerSchema(Description = "Length of the consumable in millimeters (column-wise)")]
    public double LengthMm { get; set; }
    [Range(10.0, 500.0D)]
    [SwaggerSchema(Description = "Width of the consumable in millimeters (row-wise)")]
    public double WidthMm { get; set; }
    [Range(5.0, 500.0D)]
    [SwaggerSchema(Description = "Height of the consumable in millimeters")]
    public double HeightMm { get; set; }
    [Range(0.0, 1000.0D)]
    [SwaggerSchema(Description = "Distance between the start of the consumable and the first column in millimeters")]
    public double FirstColumnOffsetMm { get; set; }
    [Range(0.0, 600.0D)]
    [SwaggerSchema(Description = "Distance between the deck and the top of the consumable in millimeters")]
    public double HeightOffDeckMm { get; set; }
}