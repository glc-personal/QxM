namespace QxM.HardwareGateway.Core.State;

public enum ConnectionState
{
    Disconnected,
    Connecting,
    Connected,
    Disconnecting,
    Faulted,
}