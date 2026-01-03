using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.Models;

namespace Qx.Domain.Labware.LabwareInstances;

public sealed class DeviceModuleLabware : Labware
{
    private DeviceModel _model;
    
    private DeviceModuleLabware(LabwareDefinition definition, LabwareId? id = null) : base(definition, id)
    {
        ArgumentNullException.ThrowIfNull(definition.DeviceModel, $"{nameof(DeviceModuleLabware)} requires a {nameof(DeviceModel)}, it cannot be null");
        _model = definition.DeviceModel;
    }

    public DeviceModuleLabware Create(LabwareDefinition definition, LabwareId? id = null)
    {
        return new DeviceModuleLabware(definition, id);
    }
}