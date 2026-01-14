namespace QxM.HardwareGateway.Core;

public sealed record ApiCommandRequest : HardwareCommandRequest
{
    public ApiCommandRequest(IdempotencyKey idempotencyKey, CorrelationId correlationId,
        string operation, ReadOnlyMemory<byte> payload, TimeSpan timeout)
        : base(CommandId.New(), idempotencyKey, correlationId, null, operation, payload, timeout)
    {
        
    }
}