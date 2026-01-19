namespace QxM.HardwareGateway.Core.Events;

public readonly record struct HardwareGatewayEventEnvelope(DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    CorrelationId? CorrelationId,
    Address? Address = null);