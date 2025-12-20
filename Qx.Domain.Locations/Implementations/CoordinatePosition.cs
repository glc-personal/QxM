using Qx.Core.Mathematics.Implementations;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Exceptions;

namespace Qx.Domain.Locations.Implementations;

/// <summary>
/// Coordinate position in 3D
/// </summary>
public sealed record CoordinatePosition : Position
{
    // possible values for x, y, and z
    private Range<double> _xRange = new Range<double>(0, double.MaxValue, true, true);
    private Range<double> _yRange = new Range<double>(0, double.MaxValue, true, true);
    private Range<double> _zRange = new Range<double>(0, double.MaxValue, true, true);

    /// <summary>
    /// Default constructor for CoordinatePosition
    /// </summary>
    public CoordinatePosition()
    {
        X = 0;
        Y = 0;
        Z = 0;
        Units = CoordinatePositionUnits.Microsteps;
    }

    public static CoordinatePosition operator +(CoordinatePosition position1, CoordinatePosition position2)
    {
        if (position1.Units != position2.Units)
            throw new NotImplementedException(
                $"Conversion between {position1.Units} and {position2.Units} is not implemented.");

        return new CoordinatePosition
        {
            X = position1.X + position2.X,
            Y = position1.Y + position2.Y,
            Z = position1.Z + position2.Z,
            Units = position1.Units,
        };
    }

    public static CoordinatePosition operator -(CoordinatePosition position1, CoordinatePosition position2)
    {
        if (position1.Units != position2.Units)
            throw new NotImplementedException($"Conversion between {position1.Units} and {position2.Units} is not implemented.");
        // TODO: replace with way to check all 3 at once to throw a better exception (needs to change exception as well)
        if (position1.X < position2.X) throw new InvalidCoordinatePositionException("X", position1.X - position2.X, position1.Units);
        if (position1.Y < position2.Y) throw new InvalidCoordinatePositionException("Y", position1.Y - position2.Y, position1.Units);
        if (position1.Z < position2.Z) throw new InvalidCoordinatePositionException("Z", position1.Z - position2.Z, position1.Units);

        return new CoordinatePosition
        {
            X = position1.X - position2.X,
            Y = position1.Y - position2.Y,
            Z = position1.Z - position2.Z,
            Units = position1.Units,
        };
    }
    
    public CoordinatePosition(double x, double y, double z, CoordinatePositionUnits units)
    {
        if (!_xRange.Contains(x)) throw new CoordinateValueOutOfRangeException(x, nameof(X), _xRange);
        X = x;
        if (!_yRange.Contains(y)) throw new CoordinateValueOutOfRangeException(y, nameof(Y), _yRange);
        Y = y;
        if (!_zRange.Contains(z)) throw new CoordinateValueOutOfRangeException(z, nameof(Z), _zRange);
        Z = z;
        Units = units;
    }
    
    // Necessary to avoid comparing the Ranges between two CoordinatePositions
    public bool Equals(CoordinatePosition? other) =>
        other is not null &&
        X == other.X &&
        Y == other.Y &&
        Z == other.Z &&
        Units == other.Units;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, Units);
    
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public CoordinatePositionUnits Units { get; init; }

    public Range<double> GetRange(CoordinateAxes coordinateAxis)
    {
        switch (coordinateAxis)
        {
            case CoordinateAxes.X:
                return _xRange;
            case CoordinateAxes.Y:
                return _yRange;
            case CoordinateAxes.Z:
                return _zRange;
            default:
                return null;
        }
    }

}