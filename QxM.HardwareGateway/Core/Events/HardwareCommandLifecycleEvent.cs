namespace QxM.HardwareGateway.Core.Events;

public sealed record HardwareCommandLifecycleEvent(
    DateTimeOffset TimestampUtc,
    HardwareId HardwareId,
    HardwareKind HardwareKind,
    CorrelationId? CorrelationId,
    Address? Address,
    CommandId CommandId,
    IdempotencyKey IdempotencyKey,
    string Operation,
    CommandStatus Status,
    HardwareError? Error)
    : HardwareEvent(TimestampUtc, HardwareId, HardwareKind, CorrelationId, Address);