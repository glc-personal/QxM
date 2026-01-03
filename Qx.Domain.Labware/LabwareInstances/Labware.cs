using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.States;

namespace Qx.Domain.Labware.LabwareInstances;

public abstract class Labware(LabwareDefinition definition, LabwareId? id = null)
{
    private LabwareLifecycleState _state = LabwareLifecycleState.Available;

    public LabwareId Id { get; } = id ?? new LabwareId(Guid.NewGuid());
    public LabwareDefinitionReference DefinitionReference { get; } = new(definition.Id, definition.Version, definition.Name);

    #region LifecycleStateMachine
    public void MarkAsAvailable() => TransitionTo(LabwareLifecycleState.Available);
    public void MarkAsReserved() => TransitionTo(LabwareLifecycleState.Reserved);
    public void MarkAsInUse() => TransitionTo(LabwareLifecycleState.InUse);
    public void MarkAsOutOfService() => TransitionTo(LabwareLifecycleState.OutOfService);
    public void MarkAsUnknown() => TransitionTo(LabwareLifecycleState.Unknown);

    private void TransitionTo(LabwareLifecycleState targetState)
    {
        if (targetState == _state) return;
        if (!IsTransitionAllowed(_state, targetState)) 
            throw new InvalidOperationException($"Invalid labware lifecycle state transition: {_state} -> {targetState}");
        OnBeforeTransition(_state, targetState);
        _state = targetState;
        OnAfterTransition(_state);
    }

    protected virtual void OnBeforeTransition(LabwareLifecycleState currentState, LabwareLifecycleState targetState) { }
    
    protected virtual void OnAfterTransition(LabwareLifecycleState currentState) { }

    private static bool IsTransitionAllowed(LabwareLifecycleState currentState, LabwareLifecycleState targetState)
    {
        return currentState switch
        {
            LabwareLifecycleState.Available
                => targetState is LabwareLifecycleState.Unknown or LabwareLifecycleState.OutOfService
                    or LabwareLifecycleState.Reserved,
            LabwareLifecycleState.Reserved 
                => targetState is LabwareLifecycleState.InUse or LabwareLifecycleState.Unknown 
                    or LabwareLifecycleState.OutOfService or LabwareLifecycleState.Available,
            LabwareLifecycleState.InUse 
                => targetState is LabwareLifecycleState.Unknown or LabwareLifecycleState.Available 
                    or LabwareLifecycleState.OutOfService,
            LabwareLifecycleState.OutOfService 
                => targetState is LabwareLifecycleState.Unknown or LabwareLifecycleState.Available,
            LabwareLifecycleState.Unknown 
                => targetState is LabwareLifecycleState.Available,
            _ => false
        };
    }
    #endregion

    protected void EnforceUseOnAvailability()
    {
        if (_state != LabwareLifecycleState.Available)
            throw new InvalidOperationException($"Cannot use {nameof(Labware)} it is not available, it is currently {_state}");
    }

    protected void EnforceUnusableLifecycleState()
    {
        if (_state is LabwareLifecycleState.Unknown or LabwareLifecycleState.OutOfService 
            or LabwareLifecycleState.Reserved or LabwareLifecycleState.InUse)
            throw new InvalidOperationException($"Cannot use {nameof(Labware)} it is not available for use, it is currently {_state}");
    }
}