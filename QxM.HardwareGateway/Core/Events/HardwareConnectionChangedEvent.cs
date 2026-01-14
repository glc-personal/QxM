using QxM.HardwareGateway.Core.State;

namespace QxM.HardwareGateway.Core.Events;

public sealed record HardwareConnectionChangedEvent(DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    ConnectionState NewState,
    string Message) 
    : HardwareEvent(TimestampUtc, HardwareId, HardwareKind);