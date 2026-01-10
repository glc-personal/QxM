namespace QxM.HardwareGateway.Core.Can;

public readonly record struct DataLengthCode
{
    public int Value { get; init; }

    public DataLengthCode(int value)
    {
        if (value is < 0 or > 8) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }
}