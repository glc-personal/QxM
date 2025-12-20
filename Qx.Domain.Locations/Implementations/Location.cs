using Qx.Core;
using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

public sealed record Location : INameable, IUniquelyIdentifiable
{
    private Position _position;
    private AxisOffset _xAxisOffset = new(CoordinateAxes.X);
    private AxisOffset _yAxisOffset = new(CoordinateAxes.Y);
    private AxisOffset _zAxisOffset = new(CoordinateAxes.Z);
    
    public Location(string name, Position position)
    {
       Name = name;
       UniqueIdentifier = Guid.NewGuid();
       _position = position;
    }

    /// <summary>
    /// Add an axis offset for when getting the position
    /// </summary>
    /// <param name="offset"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddAxisOffset(AxisOffset offset)
    {
        switch (offset.Axis)
        {
            case CoordinateAxes.X:
                _xAxisOffset = offset;
                break;
            case CoordinateAxes.Y:
                _yAxisOffset = offset;
                break;
            case CoordinateAxes.Z:
                _zAxisOffset = offset;
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown axis type: {offset.Axis}");
        }
    }

    public string Name { get; }
    public Guid UniqueIdentifier { get; }

    public Position Position
    {
        get
        {
            try
            {
                var position = (CoordinatePosition)_position;
                return new CoordinatePosition
                {
                    X = position.X + _xAxisOffset.Value,
                    Y = position.Y + _yAxisOffset.Value,
                    Z = position.Z + _zAxisOffset.Value
                };
            }
            catch (Exception)
            {
                return _position;
            }
        }
    }
}