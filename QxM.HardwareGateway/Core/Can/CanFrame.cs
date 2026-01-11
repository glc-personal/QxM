using System.Text;

namespace QxM.HardwareGateway.Core.Can;

/// <summary>
/// CAN Frame
/// </summary>
public readonly record struct CanFrame
{
    public ArbitrationId Id { get; }
    public bool IsRemoteTransmitRequest { get; }
    public ReadOnlyMemory<byte> Data { get; }
        
    private CanFrame(ArbitrationId id, bool isRemoteTransmitRequest, ReadOnlyMemory<byte> data)
    {
        EnforceRemoteFrameDataLength(isRemoteTransmitRequest, data);
        Id = id;
        Data = data;
        IsRemoteTransmitRequest = isRemoteTransmitRequest;
    }

    public static CanFrame Create(ArbitrationId id, bool isRemoteTransmitRequest, ReadOnlyMemory<byte> data)
    {
        return new CanFrame(id, isRemoteTransmitRequest, data);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder()
            .AppendLine($"Id: {Id.Value}")
            .AppendLine($"Is Remote Frame: {IsRemoteTransmitRequest}")
            .AppendLine($"Extended Frame: {Id.IsExtended}")
            .AppendLine($"Data: {Data.ToArray().ToString()}")
            .AppendLine($"Data Length Code: {Data.Length}");
        return stringBuilder.ToString();
    }

    private static void EnforceRemoteFrameDataLength(bool isRemoteTransmitRequest, ReadOnlyMemory<byte> data)
    {
        if (isRemoteTransmitRequest && data.Length != 0)
            throw new ArgumentException($"Invalid {nameof(CanFrame)}: Remote frame ({isRemoteTransmitRequest}) always contain zero data bytes ({data.Length} bytes)");
    }
}