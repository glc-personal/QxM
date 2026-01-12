namespace QxM.HardwareGateway.Core;

public sealed record HardwareCommandRequest
{
    public CommandId Id { get; }
    public IdempotencyKey IdempotencyKey { get; }
    public CorrelationId CorrelationId { get; }
    public Address Address { get; }
    public string Operation { get; }
    public ReadOnlyMemory<byte> Payload { get; }
    public TimeSpan Timeout { get; }
    
    private HardwareCommandRequest(CommandId commandId, IdempotencyKey idempotencyKey, CorrelationId correlationId,
        Address address, string operation, ReadOnlyMemory<byte> payload, TimeSpan timeout)
    {
        EnforceOperation(operation);
        EnforceTimeout(timeout);
        Id = commandId;
        IdempotencyKey = idempotencyKey;
        CorrelationId = correlationId;
        Address = address;
        Operation = operation;
        Payload = payload;
        Timeout = timeout;
    }

    public static HardwareCommandRequest Create(IdempotencyKey idempotencyKey, CorrelationId correlationId,
        Address address, string operation, ReadOnlyMemory<byte> payload, TimeSpan timeout) 
        => new HardwareCommandRequest(CommandId.New(), idempotencyKey, correlationId, address, operation, payload, timeout);
    
    private void EnforceOperation(string operation)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException($"Invalid {nameof(HardwareCommandRequest)}: the command operation cannot be null or whitespace.", nameof(operation));
    }

    private void EnforceTimeout(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException($"Invalid {nameof(HardwareCommandRequest)}: timeout ({timeout}) must be greater than zero.");
    }
}