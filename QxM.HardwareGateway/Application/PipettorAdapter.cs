using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application;

public sealed class PipettorAdapter : IHardwareAdapter
{
    public PipettorAdapter(IHardwareClient<ApiCommandRequest> hardwareClient)
    {
        HardwareId = hardwareClient.HardwareId;
        HardwareKind = hardwareClient.HardwareKind;
    }
    
    public HardwareId HardwareId { get; }
    public HardwareKind HardwareKind { get; }
    
    public Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerator<HardwareGatewayEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}