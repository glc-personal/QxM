namespace QxM.HardwareGateway.Core.Events;

public sealed record HardwareConnectionChangedEvent(DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    ConnectionState NewState) 
    : HardwareEvent(TimestampUtc, HardwareId, HardwareKind);