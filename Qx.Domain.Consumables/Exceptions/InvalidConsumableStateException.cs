using Qx.Domain.Consumables.Enums;

namespace Qx.Domain.Consumables.Exceptions;

public class InvalidConsumableStateException(ConsumableStates currentStates, ConsumableStates targetState) 
    : InvalidOperationException($"Invalid consumable state, expected {targetState} but got {currentStates}")
{
    
}