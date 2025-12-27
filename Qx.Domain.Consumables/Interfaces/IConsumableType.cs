using Qx.Core;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IConsumableType : IUniquelyIdentifiable, IVersionable
{
    ConsumableTypes Type { get; }
    ConsumableGeometry Geometry { get; }
    bool DefaultIsReusable { get; }
    int? DefaultMaxUses { get; }
}