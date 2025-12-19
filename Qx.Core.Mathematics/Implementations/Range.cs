namespace Qx.Core.Mathematics.Implementations;

public class Range<T> where T : IComparable<T>
{
    public T Minimum { get; }
    public T Maximum { get; }
    public bool IncludeMinimum { get; }
    public bool IncludeMaximum { get; }

    public Range(T minimum, T maximum, bool includeMinimum, bool includeMaximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        IncludeMinimum = includeMinimum;
        IncludeMaximum = includeMaximum;
    }

    public bool Contains(T value)
    {
        var left = false;
        var right = false;
        if (IncludeMinimum)
            left = Minimum.CompareTo(value) <= 0;
        else 
            left = value.CompareTo(Minimum) < 0;
        if (IncludeMaximum)
            right = Maximum.CompareTo(value) >= 0;
        else
            right = value.CompareTo(Maximum) > 0;
        return left && right;
    }
}