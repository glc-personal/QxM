using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;

namespace QxM.HardwareGateway.Application;

public sealed class IcbAdapter : IHardwareAdapter
{
    public IcbAdapter(TimeoutPolicy timeoutPolicy)
    {
        HardwareId = HardwareId.New;
        HardwareKind = HardwareKind.Icb;
        TimeoutPolicy = timeoutPolicy;
    }
    
    public HardwareId HardwareId { get; }
    public HardwareKind HardwareKind { get; }
    public TimeoutPolicy TimeoutPolicy { get; }

    public Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerator<HardwareGatewayEventEnvelope> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }
}