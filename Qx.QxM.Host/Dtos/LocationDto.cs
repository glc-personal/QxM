using System.ComponentModel.DataAnnotations;
using Qx.Domain.Locations.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public sealed class LocationDto
{
    [SwaggerSchema(Description = "Deck slot")]
    public SlotName Slot { get; set; }
    [SwaggerSchema(Description = "Batch")]
    public BatchNames Batch { get; set; }
    [Range(0, 12)]
    [SwaggerSchema(Description = "Column Index", Nullable = true)]
    public int ColumnIndex { get; set; }
    [SwaggerSchema(Description = "Coordinate position of the location")]
    public CoordinatePositionDto Coordinate { get; set; }
    [SwaggerSchema(Description = "Frame of reference for the location")]
    public CoordinateFrame Frame { get; set; }
    [SwaggerSchema(Description = "Boolean value indicating if this location is a custom location or a persistant one (alleviates the naming convention)")]
    public bool IsCustom { get; set; }
}