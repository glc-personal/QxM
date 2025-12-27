using System.Data.Common;
using Qx.Core;
using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

public sealed record Location : INameable, IUniquelyIdentifiable
{
    private AxisOffset _xAxisOffset = new(CoordinateAxes.X);
    private AxisOffset _yAxisOffset = new(CoordinateAxes.Y);
    private AxisOffset _zAxisOffset = new(CoordinateAxes.Z);
    
    public Location(Guid id, string name, CoordinatePosition position, CoordinateFrame frame, bool isCustom)
    {
        UniqueIdentifier = id; 
        Name = name; 
        Frame = frame;
        Position = position;
        IsCustom = isCustom;
    }

    /// <summary>
    /// Add an axis offset for when getting the position
    /// </summary>
    /// <param name="offset"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Location AddAxisOffset(AxisOffset offset)
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

        var shiftedPosition = new CoordinatePosition
        {
            X = Position.X + _xAxisOffset.Value,
            Y = Position.Y + _yAxisOffset.Value,
            Z = Position.Z + _zAxisOffset.Value,
            Units = Position.Units
        };
        var name =
            $"{Name}-{nameof(CoordinateAxes.X)}{_xAxisOffset.Value}-{nameof(CoordinateAxes.Y)}{_yAxisOffset.Value}-{nameof(CoordinateAxes.Z)}{_zAxisOffset.Value}";
        return new Location(UniqueIdentifier, name, shiftedPosition, Frame, true);
    }

    /// <summary>
    /// Name of the location
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid UniqueIdentifier { get; }
    /// <summary>
    /// Location frame of reference for the location position
    /// </summary>
    public CoordinateFrame Frame { get; }
    /// <summary>
    /// Dictates if the location is custom (alleviates naming requirement)
    /// </summary>
    public bool IsCustom { get; }
    /// <summary>
    /// Coordinate position of the location (X, Y, Z, units)
    /// </summary>
    public CoordinatePosition Position { get; }
}