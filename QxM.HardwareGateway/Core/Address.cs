namespace QxM.HardwareGateway.Core;

public readonly record struct Address
{
    public int Value { get; }
    
    public Address(int value)
    {
        Value = value;
    }
    
    private void EnforcePositiveAddress(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException($"Invalid {nameof(Address)}: hardware address cannot be negative ({value})");
    }
}