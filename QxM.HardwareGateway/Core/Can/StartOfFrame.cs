namespace QxM.HardwareGateway.Core.Can;

public readonly record struct StartOfFrame
{
    public readonly string Value;
    
    public StartOfFrame(string value)
    {
        EnforceSingleBit(value);
        Value = value;
    }

    private void EnforceSingleBit(string value)
    {
        if (value.Length > 0x1)
            throw new ArgumentOutOfRangeException($"{nameof(StartOfFrame)} value must be a single bit");
    }
}