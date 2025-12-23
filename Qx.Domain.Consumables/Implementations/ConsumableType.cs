using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Version = Qx.Core.Version;

namespace Qx.Domain.Consumables.Implementations;

public sealed class ConsumableType : IConsumableType
{
    public ConsumableType(ConsumableTypes consumableTypeEnum, int rowCount, int columnCount, string geometry)
    {
        UniqueIdentifier = Guid.NewGuid();
        Name = consumableTypeEnum.ToString();
        Version = new Version
        {
            Major = 1,
            Minor = 1,
            Build = 1,
            Revision = 1
        };
        RowCount = rowCount;
        ColumnCount = columnCount;
        Geometry = geometry;
        DefaultIsReusable = true;
        DefaultMaxUses = null;
    }
    public Guid UniqueIdentifier { get; }
    public string Name { get; }
    public Version Version { get; }
    public int RowCount { get; }
    public int ColumnCount { get; }
    public string Geometry { get; }
    public bool DefaultIsReusable { get; }
    public int? DefaultMaxUses { get; }
}