namespace QxM.HardwareGateway.Core.Events;

public readonly record struct HardwareGatewayEvent(DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    CorrelationId? CorrelationId,
    Address? Address = null);