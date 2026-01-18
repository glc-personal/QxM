using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Core.State;

namespace QxM.HardwareGateway.Infrastructure;

public sealed class PipettorClient : IHardwareClient<ApiCommandRequest>
{
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

    public Task<HardwareCommandResponse> ExecuteCommandAsync(ApiCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareCommandAccepted> SubmitCommandAsync(ApiCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<HardwareEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}