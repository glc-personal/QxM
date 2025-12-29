using Version = Qx.Core.Version;

namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Labware definition reference to reference a labware definition
/// </summary>
/// <param name="DefinitionId">LabwareDefinitionId of the LabwareDefinition</param>
/// <param name="DefinitionVersion">Version of the LabwareDefinition</param>
/// <param name="DefinitionName">Name of the LabwareDefinition</param>
public sealed record LabwareDefinitionReference(Guid DefinitionId, Version DefinitionVersion, string DefinitionName);