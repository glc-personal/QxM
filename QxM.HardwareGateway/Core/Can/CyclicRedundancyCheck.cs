namespace QxM.HardwareGateway.Core.Can;

public readonly record struct CyclicRedundancyCheck
{
    public int Value { get; init; }
    
    private CyclicRedundancyCheck(int value)
    {
        EnforceCrcIs15Bits(value);
        Value = value;
    }

    public static CyclicRedundancyCheck Create(ReadOnlyMemory<byte> data)
    {
        return new CyclicRedundancyCheck(0x7FFF);
    }

    private void EnforceCrcIs15Bits(int value)
    {
        // value must be less than 2^15-1=0x7FFF in hex (15-bits)
        if (value > 0x7FFF)  throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(CyclicRedundancyCheck)} must be a 15-bit value.");
    }
}