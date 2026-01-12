namespace QxM.HardwareGateway.Core;

public sealed record HardwareCommandResponse(CommandId CommandId, CommandStatus Status, HardwareError? Error,
    ReadOnlyMemory<byte> Payload)
{
    public bool IsTerminal =>
        Status is CommandStatus.Cancelled
            or CommandStatus.Completed
            or CommandStatus.Failed
            or CommandStatus.TimedOut
            or CommandStatus.Rejected;
}