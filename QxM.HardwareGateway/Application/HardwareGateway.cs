using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;

namespace QxM.HardwareGateway.Application;

/// <summary>
/// Hardware Gateway to the Instrument Control Board CAN bus
/// </summary>
public class HardwareGateway : IHardwareGateway
{
    private IGatewayCommunication _communication;

    public HardwareGateway(IGatewayCommunication communication)
    {
        _communication = communication;
    }
    
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await _communication.ConnectAsync(cancellationToken);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        await _communication.DisconnectAsync(cancellationToken);
    }

    public Task<SendCommandResponse> SendCommandAsync(SendCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CommandEvent> CheckCommandAsync(CheckCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}