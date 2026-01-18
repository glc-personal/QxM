using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Core.State;

namespace QxM.HardwareGateway.Infrastructure;

public interface IHardwareClient<TRequest> : IAsyncDisposable where TRequest : HardwareCommandRequest
{
    HardwareId HardwareId { get; }
    HardwareKind HardwareKind { get; }
    
    ConnectionState ConnectionState { get; }
    
    TimeoutPolicy TimeoutPolicy { get; }
    
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Heartbeat confirming basic connectivity
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HardwareHeartbeat> GetHeartbeatAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Executes and waits for a terminal response 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HardwareCommandResponse> ExecuteCommandAsync(TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Submits a command and returns quickly after device accepts.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HardwareCommandAccepted> SubmitCommandAsync(TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Subscribes to all device events
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<HardwareEvent> SubscribeAsync(CancellationToken cancellationToken = default);
}