namespace QxM.HardwareGateway.Core.Can;

public readonly record struct ArbitrationId(uint Value, bool IsExtended)
{
    public static ArbitrationId Standard(uint id)
    {
        // Standard: 11-bit (2^11-1 = 2047 -> 0x7FF) 
        if (id > 0x7FF) throw new ArgumentOutOfRangeException(nameof(id), 
            $"Standard {nameof(ArbitrationId)} cannot have an id > 2^11");
        return new ArbitrationId(id, false);
    }

    public static ArbitrationId Extended(uint id)
    {
        // Extended: 29-bit (2^29-1)
        if (id > 0x1FFFFFFF)
            throw new ArgumentOutOfRangeException(nameof(id),
                $"Extended {nameof(ArbitrationId)} cannot have an id > 0x1FFFFFFF");
        return new ArbitrationId(id, true);
    }
}