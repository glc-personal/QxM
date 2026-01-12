namespace QxM.HardwareGateway.Core;

public readonly record struct CommandId(Guid Value)
{
    public static CommandId New() => new CommandId(Guid.NewGuid());
    public override string ToString() => Value.ToString("D");
}