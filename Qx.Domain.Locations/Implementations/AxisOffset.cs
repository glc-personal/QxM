using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

public sealed record AxisOffset(CoordinateAxes Axis,
    double Value = 0, 
    CoordinatePositionUnits Units = CoordinatePositionUnits.Microsteps);
