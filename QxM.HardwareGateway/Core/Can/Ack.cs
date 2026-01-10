namespace QxM.HardwareGateway.Core.Can;

public readonly record struct Ack(bool IsNotAcknowledged)
{
    public static Ack Dominant => new Ack(false);
    public static Ack Recessive => new Ack(true);
}