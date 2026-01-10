namespace QxM.HardwareGateway.Core.Can;

public readonly record struct IdentifierExtension(bool IsExtended)
{
    public static IdentifierExtension Dominant => new IdentifierExtension(false);
    public static IdentifierExtension Recessive => new IdentifierExtension(true);
}