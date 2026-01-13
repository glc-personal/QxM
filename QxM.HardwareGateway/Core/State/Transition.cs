namespace QxM.HardwareGateway.Core.State;

public sealed record Transition<TState, TTrigger>(TState From, TState To, TTrigger Trigger);