using Qx.Core;

namespace Qx.Domain.Consumables.Interfaces;

public interface IConsumableType : IUniquelyIdentifiable, INameable, IVersionable
{
    int RowCount { get; }
    int ColumnCount { get; }
    string Geometry { get; }
    bool DefaultIsReusable { get; }
    int? DefaultMaxUses { get; }
}