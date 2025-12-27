namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Labware kind
/// </summary>
public enum LabwareKind
{
    DeviceModule,       // device labware (communication with a real device or simulator)
    Fixture,            // fixture labware (nests, adapters, clamps, carriers, etc)
    TipContainer,       // tip containing labware
    LiquidContainer,    // liquid containing labware
}