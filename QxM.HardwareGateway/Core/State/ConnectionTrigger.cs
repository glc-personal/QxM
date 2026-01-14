namespace QxM.HardwareGateway.Core.State;

public enum ConnectionTrigger
{
    Connect,
    ConnectSucceeded,
    ConnectFailed,
    Disconnect,
    DisconnectSucceeded,
    DisconnectFailed,
    Fault,
    ResetFault
}