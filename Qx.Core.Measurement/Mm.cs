using System.Xml.Xsl;

namespace Qx.Core.Measurement;

/// <summary>
/// Measurement value in millimeters
/// </summary>
public readonly record struct Mm
{
    // ReSharper disable once MemberCanBePrivate.Global
    public Mm(decimal value)
    {
        EnsureNonNegative(value);
        Value = value;
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    public decimal Value { get; }
    
    public override string ToString()
        => $"{Value} mm";
    
    public static Mm operator+(Mm a, Mm b)
        => new Mm(a.Value + b.Value);
    
    public static Mm operator-(Mm a, Mm b)
        => new Mm(a.Value - b.Value);

    public bool Equals(Mm other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static Mm Zero => new(0);
    
    private static void EnsureNonNegative(decimal value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Value cannot be negative.");
    }
}