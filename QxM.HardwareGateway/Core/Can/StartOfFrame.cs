namespace QxM.HardwareGateway.Core.Can;

public readonly record struct StartOfFrame
{
    public readonly int Value;
    
    public StartOfFrame(int value)
    {
        EnforceSingleBit(value);
        Value = value;
    }

    private void EnforceSingleBit(int value)
    {
        if (value > 0x1)
            throw new ArgumentOutOfRangeException($"{nameof(StartOfFrame)} value must be a single bit");
    }
}