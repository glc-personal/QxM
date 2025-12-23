using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Version = Qx.Core.Version;

namespace Qx.Domain.Consumables.Implementations;

public sealed class ConsumableType(
    ConsumableTypes consumableTypeEnum,
    ConsumableGeometry geometry,
    Version version,
    bool defaultIsReusable = true,
    int? defaultMaxUses = null)
    : IConsumableType
{
    public Guid UniqueIdentifier { get; } = Guid.NewGuid();
    public string Name { get; } = consumableTypeEnum.ToString();
    public Version Version { get; } = version;
    public ConsumableGeometry Geometry { get; } = geometry;
    public bool DefaultIsReusable { get; } = defaultIsReusable;
    public int? DefaultMaxUses { get; } = defaultMaxUses;
}