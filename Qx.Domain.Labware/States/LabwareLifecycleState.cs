namespace Qx.Domain.Labware.States;

public enum LabwareLifecycleState
{
    Available,      // labware is available to use
    Reserved,       // labware is reserved for future use (available -> reserved)
    InUse,           // labware is currently busy with a long-running task (applies to labware devices, nothing else atm)
    OutOfService,   // labware is out of service (placed out of service, never goes there on its own)
    Unknown,        // labware state is unknown due to an error or failure
}