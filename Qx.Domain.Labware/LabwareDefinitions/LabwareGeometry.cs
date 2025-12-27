namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Describes how much space the labware takes up and any grid
/// </summary>
/// <param name="labwareEnvelope">How much space the labware takes up</param>
/// <param name="labwareGrid?">Any grid definition</param>
public sealed class LabwareGeometry(LabwareEnvelope labwareEnvelope, LabwareGrid? labwareGrid)
{
    public LabwareEnvelope Envelope { get; } = labwareEnvelope;
    public LabwareGrid? Grid { get; } = labwareGrid;
}