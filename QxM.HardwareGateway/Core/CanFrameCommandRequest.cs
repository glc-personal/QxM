using QxM.HardwareGateway.Core.Can;

namespace QxM.HardwareGateway.Core;

public sealed record CanFrameCommandRequest : HardwareCommandRequest
{
    public CanFrameCommandRequest(IdempotencyKey idempotencyKey, CorrelationId correlationId,
        Address address, string operation, ReadOnlyMemory<byte> payload, TimeSpan timeout,
        StartOfFrame startOfFrame, EndOfFrame endOfFrame, bool isRemoteTransmitRequest, bool isExtended)
        : base(CommandId.New(), idempotencyKey, correlationId, address, operation, payload, timeout)
    {
        CanFrame = new CanFrame(startOfFrame, new ArbitrationId(address.Value, isExtended), 
            isRemoteTransmitRequest, payload, endOfFrame);
    }

    public CanFrame CanFrame { get; }
}