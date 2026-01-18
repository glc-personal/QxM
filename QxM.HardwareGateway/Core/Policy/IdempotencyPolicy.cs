namespace QxM.HardwareGateway.Core.Policy;

public sealed record IdempotencyPolicy(TimeSpan Expiration)
{
    public static IdempotencyPolicy Default => new IdempotencyPolicy(TimeSpan.FromSeconds(5));
}