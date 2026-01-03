using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Labware.LabwareInstances;

public sealed class FixtureLabware : Labware
{
    private FixtureLabware(LabwareDefinition definition, LabwareId? id = null) : base(definition, id)
    {
        
    }

    public FixtureLabware Create(LabwareDefinition definition, LabwareId? id = null)
    {
        return new FixtureLabware(definition, id);
    }
}