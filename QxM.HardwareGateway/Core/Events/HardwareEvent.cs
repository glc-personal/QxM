namespace QxM.HardwareGateway.Core.Events;

public abstract record HardwareEvent(DateTimeOffset TimestampUtc,
    HardwareId HardwareId, 
    HardwareKind HardwareKind,
    CorrelationId? CorrelationId = null,
    Address? Address = null);