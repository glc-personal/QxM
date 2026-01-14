namespace QxM.HardwareGateway.Core.State;

public sealed record StateChanged<TState, TTrigger>(TState From, TState To, TTrigger Trigger, string? Reason) 
    where TState : Enum;