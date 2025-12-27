namespace Qx.QxM.Host.Dtos;

public sealed class CreateConsumableDto
{
    public Guid ConsumableTypeId { get; set; }
    public Guid LocationId { get; set; }
    public ReusePolicyDto ReusePolicy { get; set; } = null!;
    public SealPolicyDto SealPolicy { get; set; } = null!;
}