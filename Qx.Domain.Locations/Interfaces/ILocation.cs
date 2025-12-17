using Qx.Core;

namespace Qx.Domain.Locations.Interfaces;

public interface ILocation : IIdentifiable, INameable
{
    public Position Position { get; }
}