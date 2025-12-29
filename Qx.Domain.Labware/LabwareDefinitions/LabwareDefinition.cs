using Qx.Core;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.Policies;
using Version = Qx.Core.Version;

namespace Qx.Domain.Labware.LabwareDefinitions;

public sealed class LabwareDefinition : IUniquelyIdentifiable, INameable
{
    internal LabwareDefinition(Guid id, string name, LabwareKind kind, Version version, LabwareGeometry geometry,
        TipContainerModel? tipModel, LiquidContainerModel? liquidContainerModel, DeviceRole? deviceRole)
    {
        Id = id;
        Name = name;
        Kind = kind;
        Version = version;
        Geometry = geometry;
        TipModel = tipModel;
        LiquidContainerModel = liquidContainerModel;
        DeviceRole = deviceRole;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public LabwareKind Kind { get; init; }
    public Version Version { get; init; }
    public LabwareGeometry Geometry { get; init; }
    public TipContainerModel? TipModel { get; init; }
    public LiquidContainerModel? LiquidContainerModel { get; init; }
    public DeviceRole? DeviceRole { get; init; }

    /// <summary> Creates a Labware Definition </summary>
    public static LabwareDefinition Create(string name, LabwareKind kind, Version version, LabwareGeometry geometry, 
        TipContainerModel? tipModel, LiquidContainerModel? liquidContainerModel, DeviceRole? deviceRole, Guid? id = null)
    {
        EnforceDomainInvariants(name, kind, geometry, tipModel, liquidContainerModel, deviceRole);
        return new LabwareDefinition(id ?? Guid.NewGuid(), name, kind, version, geometry, 
            tipModel, liquidContainerModel, deviceRole);
    }

    // Enforce domain invariants (business rules)
    private static void EnforceDomainInvariants(string name, LabwareKind kind, LabwareGeometry geometry,
        TipContainerModel? tipModel, LiquidContainerModel? liquidContainerModel, DeviceRole? deviceRole)
    {
        if (string.IsNullOrEmpty(name)) 
            throw new ArgumentException($"{nameof(LabwareDefinition)}.{nameof(Name)} cannot be null or empty and got: {name}");
        switch (kind)
        {
            case LabwareKind.TipContainer:
                EnsureGeometryHasGrid(kind, geometry);
                if (tipModel == null) throw new ArgumentNullException($"{kind} labware requires {nameof(TipModel)}");
                break;
            case LabwareKind.LiquidContainer:
                EnsureGeometryHasGrid(kind, geometry);
                if (liquidContainerModel == null) throw new ArgumentNullException($"{kind} labware requires {nameof(LiquidContainerModel)}");
                break;
            case LabwareKind.DeviceModule:
                if (deviceRole == null) throw new ArgumentNullException($"{kind} labware requires {nameof(DeviceRole)}");
                break;
            case LabwareKind.Fixture:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown labware kind");
        }
    }
    
    private static void EnsureGeometryHasGrid(LabwareKind kind, LabwareGeometry geometry)
    {
        if (geometry.Grid == null) throw new ArgumentException($"{kind} labware requires {nameof(Geometry)} with {nameof(Geometry.Grid)}");
    }
}