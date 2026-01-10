using System.Text;

namespace QxM.HardwareGateway.Infrastructure;

/// <summary>
/// Gateway Communication Configuration Options
/// </summary>
public sealed class GatewayCommunicationOptions
{
    public bool IsSimulated { get; init; }
    public string? ComPort { get; init; }
    public int BaudRate { get; init; }
    public string CanInterface { get; init; } = null!;
    public int TimeoutMs  { get; init; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder()
            .AppendLine($"IsSimulated: {IsSimulated}")
            .AppendLine($"ComPort: {ComPort}")
            .AppendLine($"BaudRate: {BaudRate}")
            .AppendLine($"CanInterface: {CanInterface}")
            .AppendLine($"TimeoutMs: {TimeoutMs}");
        return stringBuilder.ToString();
    }
}