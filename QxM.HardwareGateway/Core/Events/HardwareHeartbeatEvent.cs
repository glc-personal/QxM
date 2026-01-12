namespace QxM.HardwareGateway.Core.Events;

public sealed record HardwareHeartbeatEvent(
    DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    HardwareHeartbeat Heartbeat) 
    : HardwareEvent(TimestampUtc, HardwareId, HardwareKind);