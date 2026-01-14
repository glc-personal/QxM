namespace QxM.HardwareGateway.Core.State;

public sealed record TriggerMessage<TTrigger>(TTrigger Trigger, string? Reason);