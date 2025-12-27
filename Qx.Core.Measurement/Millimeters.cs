using System.Xml.Xsl;

namespace Qx.Core.Measurement;

/// <summary>
/// Measurement value in millimeters
/// </summary>
public readonly record struct Millimeters
{
    // ReSharper disable once MemberCanBePrivate.Global
    public Millimeters(decimal value)
    {
        EnsureNonNegative(value);
        Value = value;
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    public decimal Value { get; }
    
    public override string ToString()
        => $"{Value} mm";
    
    public static Millimeters operator+(Millimeters a, Millimeters b)
        => new Millimeters(a.Value + b.Value);
    
    public static Millimeters operator-(Millimeters a, Millimeters b)
        => new Millimeters(a.Value - b.Value);

    public bool Equals(Millimeters other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static Millimeters Zero => new(0);
    
    private static void EnsureNonNegative(decimal value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Value cannot be negative.");
    }
}