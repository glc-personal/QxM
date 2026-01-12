namespace QxM.HardwareGateway.Core;

public sealed record HardwareCommandAccepted(CommandId CommandId, DateTimeOffset AcceptedAtUtc);