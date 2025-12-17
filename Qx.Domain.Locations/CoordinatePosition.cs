using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations;

/// <summary>
/// Coordinate position in 3D
/// </summary>
/// <param name="X">Along the x-axis</param>
/// <param name="Y">Along the y-axis</param>
/// <param name="Z">Along the z-axis</param>
public sealed record CoordinatePosition(double X, double Y, double Z, CoordinatePositionUnits Units) 
    : Position;