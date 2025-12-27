using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Locations.Exceptions;

public class LocationPositionTypeException(Type locationPositionType) 
    : InvalidCastException($"Invalid location position type cast ({locationPositionType}), expected {nameof(CoordinatePosition)}")
{
    
}