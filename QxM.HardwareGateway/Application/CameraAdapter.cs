using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application;

public sealed class CameraAdapter(IHardwareClient<ApiCommandRequest> HardwareClient) : IHardwareAdapter
{
    public HardwareId HardwareId => HardwareClient.HardwareId;
    public HardwareKind HardwareKind => HardwareClient.HardwareKind;
    
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