namespace QxM.HardwareGateway.Core.Responses;

public readonly record struct HardwareGatewayCommandResponseEnvelope(CommandId CommandId,
    CommandStatus CommandStatus,
    HardwareError? Error,
    ReadOnlyMemory<byte> Payload);