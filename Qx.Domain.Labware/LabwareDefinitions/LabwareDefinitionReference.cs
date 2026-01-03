using Version = Qx.Core.Version;

namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Labware definition reference to reference a labware definition
/// </summary>
/// <param name="Id">LabwareDefinitionId of the LabwareDefinition</param>
/// <param name="Version">Version of the LabwareDefinition</param>
/// <param name="Name">Name of the LabwareDefinition</param>
public sealed record LabwareDefinitionReference(LabwareId Id, Version Version, string Name);