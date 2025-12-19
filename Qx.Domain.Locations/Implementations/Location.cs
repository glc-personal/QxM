using Qx.Core;

namespace Qx.Domain.Locations.Implementations;

public sealed record Location : IIdentifiable, INameable
{
    public Location(string name, int id, CoordinatePosition position)
    {
       Id = id;
       Name = name;
       Position = position;
    }
    
    public static Location operator+(Location location, CoordinatePosition position)
        => new Location(location.Name, location.Id, location.Position + position);

    public static Location operator -(Location location, CoordinatePosition position) 
        => new Location(location.Name, location.Id, location.Position - position);

    public int Id { get; }
    public string Name { get; }
    public CoordinatePosition Position { get; }
}