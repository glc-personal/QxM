namespace QxM.HardwareGateway.Core.Can;

public readonly record struct CyclicRedundancyCheck
{
    public int Value { get; }

    public CyclicRedundancyCheck(ReadOnlyMemory<byte> data)
    {
        var bitCount = data.Length / 8;
        EnforceBitCount(bitCount);
        Value = 0;
    }

    private void EnforceBitCount(int value)
    {
        if (value <= 0x7FFF)
            throw new ArgumentException($"{nameof(CyclicRedundancyCheck)} must be 15-bits");
    }
}