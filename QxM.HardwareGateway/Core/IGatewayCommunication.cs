using QxM.HardwareGateway.Core.Can;

namespace QxM.HardwareGateway.Core;

public interface IGatewayCommunication
{
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    Task Write(CanFrame frame, CancellationToken cancellationToken = default);
    Task<CanFrame> ReadAsync(CancellationToken cancellationToken = default);
}