using System.Text;

namespace QxM.HardwareGateway.Core.Can;

/// <summary>
/// CAN Frame
/// </summary>
public readonly record struct CanFrame
{
    public ArbitrationId Id { get; }
    public RemoteTransmitRequest Rtr { get; }
    public IdentifierExtension Extension { get; }
    public DataLengthCode Dlc { get; }
    public ReadOnlyMemory<byte> Data { get; }
    public CyclicRedundancyCheck Crc { get; }
    public Ack Ack { get; }
        
    private CanFrame(ArbitrationId id, RemoteTransmitRequest rtr, IdentifierExtension extension, 
        ReadOnlyMemory<byte> data, CyclicRedundancyCheck crc, Ack ack)
    {
        EnforceIdentifierExtensionAndArbitrationId(extension, id);
        EnforceRemoteFrameDataLength(rtr, data);
        Id = id;
        Rtr = rtr;
        Extension = extension;
        Dlc = new DataLengthCode(data.Length);
        Data = data;
        Crc = crc;
        Ack = ack;
    }

    public static CanFrame Create(ArbitrationId id, RemoteTransmitRequest rtr, IdentifierExtension extension,
        ReadOnlyMemory<byte> data, CyclicRedundancyCheck crc, Ack ack)
    {
        return new CanFrame(id, rtr, extension, data, crc, ack);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder()
            .AppendLine($"Id: {Id.Value}")
            .AppendLine($"Rtr: {Rtr.IsRemoteFrame}")
            .AppendLine($"Extension: {Extension.IsExtended}")
            .AppendLine($"Data: {Data.ToArray().ToString()}")
            .AppendLine($"Crc: {Crc.Value}")
            .AppendLine($"Ack: {Ack.IsNotAcknowledged}");
        return stringBuilder.ToString();
    }

    private static void EnforceRemoteFrameDataLength(RemoteTransmitRequest rtr, ReadOnlyMemory<byte> data)
    {
        if (rtr.IsRemoteFrame && data.Length != 0)
            throw new ArgumentException($"Invalid {nameof(CanFrame)}: Remote frame ({rtr.IsRemoteFrame}) always contain zero data bytes ({data.Length} bytes)");
    }

    private static void EnforceIdentifierExtensionAndArbitrationId(IdentifierExtension identifierExtension, ArbitrationId arbitrationId)
    {
        if ((identifierExtension.IsExtended && !arbitrationId.IsExtended) ||
            (!identifierExtension.IsExtended && arbitrationId.IsExtended))
            throw new ArgumentException($"Invalid {nameof(CanFrame)}: Arbitration ID (extended: {arbitrationId.IsExtended}) and Identifier Extension (extended: {identifierExtension.IsExtended}) must be standard or both extended.");
    }
}