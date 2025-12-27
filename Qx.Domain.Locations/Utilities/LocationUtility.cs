using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Exceptions;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Locations.Utilities;

public static class LocationUtility
{
    /// <summary>
    /// Create a new shifted location
    /// </summary>
    /// <param name="locationPosition">Location to be shifted</param>
    /// <param name="shiftX">Amount to shift the location by along the x-axis (assuming the same units as the location)</param>
    /// <param name="shiftY">Amount to shift the location by along the y-axis (assuming the same units as the location)</param>
    /// <param name="shiftZ">Amount to shift the location by along the y-axis (assuming the same units as the location)</param>
    /// <param name="frame">Frame of reference</param>
    /// <param name="shiftLocationName">Name of the created shifted location</param>
    /// <returns></returns>
    /// <exception cref="CoordinateValueOutOfRangeException"></exception>
    //public static Location ShiftLocation(CoordinatePosition locationPosition, double shiftX, double shiftY, double shiftZ,
    //    LocationFrames frame ,string shiftLocationName)
    //{
    //    var futureX = shiftX + locationPosition.X;
    //    var futureY = shiftY + locationPosition.Y;
    //    var futureZ = shiftZ + locationPosition.Z;
    //    if (futureX < 0) 
    //        throw new CoordinateValueOutOfRangeException(futureX, CoordinateAxes.X.ToString(), locationPosition.GetRange(CoordinateAxes.X));
    //    if (futureY < 0)
    //        throw new CoordinateValueOutOfRangeException(futureY, CoordinateAxes.Y.ToString(), locationPosition.GetRange(CoordinateAxes.Y));
    //    if (futureZ < 0)
    //        throw new CoordinateValueOutOfRangeException(futureZ, CoordinateAxes.Z.ToString(), locationPosition.GetRange(CoordinateAxes.Z));
    //    return new Location(Guid.NewGuid(), shiftLocationName, new CoordinatePosition
    //    {
    //        X = futureX,
    //        Y = futureY,
    //        Z = futureZ,
    //        Units = locationPosition.Units
    //    }, frame);
    //}
    
    public static CoordinatePosition ConvertCoordinatePosition(CoordinatePositionUnits units,
        CoordinatePosition coordinatePosition)
    {
        throw new NotImplementedException();
    }

    private static double ConvertMicrostepToMillimeter(double microstep)
    {
        throw new NotImplementedException();
    }
}