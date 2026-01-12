namespace QxM.HardwareGateway.Core;

public readonly record struct HardwareId(Guid Value)
{
    public static HardwareId New => new HardwareId(Guid.NewGuid());
    public override string ToString() => Value.ToString("D");
}