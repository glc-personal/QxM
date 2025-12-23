using Qx.Core;
using Qx.Domain.Consumables.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IConsumableType : IUniquelyIdentifiable, INameable, IVersionable
{
    ConsumableGeometry Geometry { get; }
    bool DefaultIsReusable { get; }
    int? DefaultMaxUses { get; }
}