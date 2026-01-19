using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;

namespace QxM.HardwareGateway.Application;

public interface IHardwareAdapter : IAsyncDisposable
{
    HardwareId HardwareId { get; }
    HardwareKind HardwareKind { get; }
    
    Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, 
        CancellationToken cancellationToken = default);
    
    Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope,
        CancellationToken cancellationToken = default);
    
    IAsyncEnumerator<HardwareGatewayEventEnvelope> SubscribeAsync(CancellationToken cancellationToken = default);
}