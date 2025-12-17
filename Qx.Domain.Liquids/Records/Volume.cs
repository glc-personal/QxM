using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Exceptions;

namespace Qx.Domain.Liquids.Records;

public readonly record struct Volume
{
    public double Value { get; init; }
    public VolumeUnits Units { get; init; }
    
    public Volume(double value, VolumeUnits units)
    {
        Value = value;
        Units = units;
        
        if (value < 0)
            throw new InvalidVolumeException(this);
    }
    
    public static Volume operator +(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        return left with { Value = left.Value + right.Value };
    }
    
    public static Volume operator -(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        if (left.Value < right.Value)
            return left with { Value = 0.0};
        return left with { Value = left.Value - right.Value };
    }

    public static bool operator <(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        return left.Value < right.Value;
    }

    public static bool operator >(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        return left.Value > right.Value;
    }

    public static bool operator <=(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        return left.Value <= right.Value;
    }

    public static bool operator >=(Volume left, Volume right)
    {
        if (left.Units != right.Units)
            throw new InvalidOperationException("Volume must have the same units.");
        return left.Value >= right.Value;
    }

    public override string ToString()
    {
        return $"{Value} ({Units})";
    }

    public Volume ToUnits(VolumeUnits units)
    {
        if (Units == units) return this;
        return units switch
        {
            VolumeUnits.Ul when Units == VolumeUnits.Ml 
                => new Volume(Value * 1000, VolumeUnits.Ul),
            VolumeUnits.Ml when Units == VolumeUnits.Ul
                => new Volume(Value / 1000, VolumeUnits.Ml),
            _ => throw new NotSupportedException($"Volume units '{units}' is not supported."),
        };
    }
}