using Qx.Core.Mathematics.Implementations;

namespace Qx.Domain.Locations.Exceptions;

public class CoordinateValueOutOfRangeException : ArgumentOutOfRangeException
{
    private string message;
    public CoordinateValueOutOfRangeException(double value, string nameOf, Range<double> range) 
        : base($"Coordinate value out of range ({range.Minimum} {GetBoundString(range.IncludeMinimum)} {nameOf} {GetBoundString(range.IncludeMaximum)} {range.Maximum}))")
    {
    }

    private static string GetBoundString(bool inclusive)
    {
        if (inclusive)
            return "<=";
        return "<";
    }
}