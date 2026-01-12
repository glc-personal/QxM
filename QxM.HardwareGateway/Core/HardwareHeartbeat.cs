namespace QxM.HardwareGateway.Core;

public sealed record HardwareHeartbeat(
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    DateTimeOffset HeartbeatTimeStampUtc,
    HeartbeatStatus Status,
    TimeSpan HeartbeatRoundTrip,
    ConnectionState State,
    string FirmwareVersion,
    HardwareError? Error)
{
    public bool IsHealthy => Status is HeartbeatStatus.Ok or HeartbeatStatus.Degraded;
}