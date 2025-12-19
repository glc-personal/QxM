using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Exceptions;

public class InvalidCoordinatePositionException(string nameOf, double value, CoordinatePositionUnits units) 
    : InvalidOperationException($"Invalid coordinate position ({value} {units}) for the {nameOf}-axis")
{
    
}