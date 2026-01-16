using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;

namespace QxM.HardwareGateway.Application.Simulators;

public sealed class SimulatedIcbAdapter : IHardwareAdapter
{
    public SimulatedIcbAdapter()
    {
        HardwareId = HardwareId.New;
        HardwareKind = HardwareKind.Icb;
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

    /// <summary>
    /// Convert a <see cref="HardwareGatewayCommandRequestEnvelope"/> to a hardware specific
    /// <see cref="HardwareCommandRequest"/>
    /// </summary>
    /// <param name="envelope"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private HardwareCommandRequest ConvertToHardwareSpecific(
        HardwareGatewayCommandRequestEnvelope envelope)
    {
        throw new NotImplementedException();
    }
    
    private HardwareCommandResponse ConvertToHardwareSpecific(
        HardwareGatewayCommandResponseEnvelope envelope)
    {
        throw new NotImplementedException();
    }
    
    private HardwareCommandAccepted ConvertToHardwareSpecific(
        HardwareGatewayCommandAcceptedEnvelope envelope)
    {
        throw new NotImplementedException();
    }
    
    private HardwareEvent ConvertToHardwareSpecific(
        HardwareGatewayEvent gatewayEvent)
    {
        throw new NotImplementedException();
    }

    public HardwareGatewayCommandRequestEnvelope ConvertToGatewaySpecific(
        HardwareCommandRequest request)
    {
        throw new NotImplementedException();
    }
    
    public HardwareGatewayCommandResponseEnvelope ConvertToGatewaySpecific(
        HardwareCommandResponse response)
    {
        throw new NotImplementedException();
    }
    
    public HardwareGatewayCommandAcceptedEnvelope ConvertToGatewaySpecific(
        HardwareCommandAccepted response)
    {
        throw new NotImplementedException();
    }
    
    public HardwareGatewayEvent ConvertToGatewaySpecific(
        HardwareEvent hardwareEvent)
    {
        throw new NotImplementedException();
    }
}