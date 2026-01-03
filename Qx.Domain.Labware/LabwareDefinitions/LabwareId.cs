namespace Qx.Domain.Labware.LabwareDefinitions;

public readonly record struct LabwareId(Guid Id)
{
    public static LabwareId New() => new(Guid.NewGuid());
}