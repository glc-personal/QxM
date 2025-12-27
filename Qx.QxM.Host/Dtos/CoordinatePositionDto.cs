using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public sealed class CoordinatePositionDto
{
    [Range(0, double.MaxValue)]
    [SwaggerSchema(Description = "Coordinate position along the x-axis")]
    public double X { get; set; }
    [Range(0, double.MaxValue)]
    [SwaggerSchema(Description = "Coordinate position along the y-axis")]
    public double Y { get; set; }
    [Range(0, double.MaxValue)]
    [SwaggerSchema(Description = "Coordinate position along the z-axis")]
    public double Z { get; set; }
}