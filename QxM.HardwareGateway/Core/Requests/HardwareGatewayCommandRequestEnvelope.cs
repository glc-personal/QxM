namespace QxM.HardwareGateway.Core.Requests;

public readonly record struct HardwareGatewayCommandRequestEnvelope(
    IdempotencyKey IdempotencyKey,
    CorrelationId CorrelationId,
    Address? Address,
    string Operation,
    ReadOnlyMemory<byte> Payload,
    TimeSpan Timeout);