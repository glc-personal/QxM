namespace QxM.HardwareGateway.Core;

public readonly record struct IdempotencyKey(Guid Value)
{
    public static IdempotencyKey New() => new IdempotencyKey(Guid.NewGuid());
    public override string ToString() => Value.ToString("D");
}