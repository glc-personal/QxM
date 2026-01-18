using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Core.State;

namespace QxM.HardwareGateway.Infrastructure;

public sealed class IcbClient : IHardwareClient<CanFrameCommandRequest>, IAsyncDisposable
{
    public IcbClient(TimeoutPolicy timeoutPolicy)
    {
        HardwareId = HardwareId.New;
        HardwareKind = HardwareKind.Icb;
        TimeoutPolicy = timeoutPolicy;
        ConnectionState = ConnectionState.Disconnected;
    }
    
    public HardwareId HardwareId { get; }
    public HardwareKind HardwareKind { get; }
    public ConnectionState ConnectionState { get; }
    public TimeoutPolicy TimeoutPolicy { get; }
    
    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareHeartbeat> GetHeartbeatAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareCommandResponse> ExecuteCommandAsync(CanFrameCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareCommandAccepted> SubmitCommandAsync(CanFrameCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<HardwareEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }
}