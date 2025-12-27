using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public sealed class SealPolicyDto
{
    [SwaggerSchema(Description = "Boolean value for the seal policy")]
    public bool IsSealed { get; set; }
}