namespace QxM.HardwareGateway.Core;

public readonly record struct CorrelationId(Guid Value)
{
    public static CorrelationId New() => new CorrelationId(Guid.NewGuid());
    public override string ToString() => Value.ToString("D");
}