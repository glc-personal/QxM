namespace QxM.HardwareGateway.Core.Responses;

public sealed record HardwareCommandAccepted(CommandId CommandId, DateTimeOffset AcceptedAtUtc);