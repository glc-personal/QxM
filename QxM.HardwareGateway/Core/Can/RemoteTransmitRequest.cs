namespace QxM.HardwareGateway.Core.Can;

public readonly record struct RemoteTransmitRequest(bool IsRemoteFrame)
{
    public static RemoteTransmitRequest Dominant => new RemoteTransmitRequest(false);
    public static RemoteTransmitRequest Recessive => new RemoteTransmitRequest(true);
}