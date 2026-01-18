namespace QxM.HardwareGateway.Core.Responses;

public readonly record struct HardwareGatewayCommandAcceptedEnvelope(CommandId CommandId, DateTimeOffset AcceptedAtUtc);