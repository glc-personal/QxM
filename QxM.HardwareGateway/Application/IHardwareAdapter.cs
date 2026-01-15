using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;

namespace QxM.HardwareGateway.Application;

public interface IHardwareAdapter
{
    HardwareId HardwareId { get; }
    HardwareKind HardwareKind { get; }
    
    Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, 
        CancellationToken cancellationToken = default);
    
    Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope,
        CancellationToken cancellationToken = default);
    
    IAsyncEnumerator<HardwareGatewayEvent> SubscribeAsync(CancellationToken cancellationToken = default);
}