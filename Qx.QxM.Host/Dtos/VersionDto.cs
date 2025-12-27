using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public sealed class VersionDto
{
    [Range(0, int.MaxValue)]
    [SwaggerSchema(Description = "Major version number")]
    public int Major { get; set; }
    [Range(0, int.MaxValue)]
    [SwaggerSchema(Description = "Minor version number")]
    public int Minor { get; set; }
    [Range(0, int.MaxValue)]
    [SwaggerSchema(Description = "Build number")]
    public int Build { get; set; }
    [Range(0, int.MaxValue)]
    [SwaggerSchema(Description = "Revision number")]
    public int Revision { get; set; }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}.{Revision}";
    }
}