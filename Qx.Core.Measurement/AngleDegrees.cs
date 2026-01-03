namespace Qx.Core.Measurement;

public record AngleDegrees
{
    public AngleDegrees(decimal value)
    {
        if (Math.Abs(value) > 360m)
            throw new ArgumentOutOfRangeException($"{nameof(AngleDegrees)} value must be between 0 and 360, but was {value}");
        Value = value;
    }
    
    public static AngleDegrees Zero => new(0);
    
    public decimal Value { get; set; }
}