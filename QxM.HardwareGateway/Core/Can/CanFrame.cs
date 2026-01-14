namespace QxM.HardwareGateway.Core.Can;

public readonly record struct CanFrame
{
    public StartOfFrame StartOfFrame { get; }
    public ArbitrationId ArbitrationId { get; }
    public bool IsRemoteTransmitRequest { get; }
    public int DataLengthCode { get; }
    public ReadOnlyMemory<byte> Data { get; }
    public CyclicRedundancyCheck CyclicRedundancyCheck { get; }
    public EndOfFrame EndOfFrame { get; }

    public CanFrame(StartOfFrame startOfFrame, ArbitrationId arbitrationId, bool isRemoteTransmitRequest,
        ReadOnlyMemory<byte> data, EndOfFrame endOfFrame)
    {
        EnforceRemoteFrameAndDataLength(isRemoteTransmitRequest, data);
        StartOfFrame = startOfFrame;
        ArbitrationId = arbitrationId;
        IsRemoteTransmitRequest = isRemoteTransmitRequest;
        Data = data;
        DataLengthCode = data.Length;
        CyclicRedundancyCheck = new CyclicRedundancyCheck(data);
        EndOfFrame = endOfFrame;
    }
    
    private void EnforceRemoteFrameAndDataLength(bool isRemoteTransmitRequest, ReadOnlyMemory<byte> data)
    {
        if (isRemoteTransmitRequest && data.Length > 0)
            throw new ArgumentException($"Remote Frames must contain zero data bytes");
    }
}