using Swashbuckle.AspNetCore.Annotations;

namespace Qx.QxM.Host.Dtos;

public class ReusePolicyDto
{
    [SwaggerSchema(Description = "Bool value indicating if this is reusable")]
    public bool IsReusable { get; set; }
    
    [SwaggerSchema(Description = "Nullable int value indicating the number of uses if reusable (null = unlimited)")]
    public int? MaxUses { get; set; }
}