namespace QxM.HardwareGateway.Core.Can;

public readonly record struct ArbitrationId
{
    public readonly int Value;
    public readonly bool IsExtended;
    
    public ArbitrationId(int value, bool isExtended)
    {
        EnforceExtendedLogic(value);
        Value = value;
        IsExtended = isExtended;
    }
    
    private void EnforceExtendedLogic(int value)
    {
        switch (IsExtended)
        {
            case true when value > 0x1FFFFFFF:
                throw new ArgumentException($"Extended {nameof(ArbitrationId)} cannot be larger than 29-bits");
            case false when value < 0x7FF:
                throw new ArgumentException($"Standard {nameof(ArbitrationId)} cannot be larger than 11-bits");
        }
    }
}