using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application.Simulators;

public sealed class SimulatedIcbAdapter(
    TimeoutPolicy timeoutPolicy,
    IHardwareClient<CanFrameCommandRequest> hardwareClient)
    : IHardwareAdapter
{
    // TODO: Make these StartOfFrame, EndOfFrame, and IsExtended configurable since they are fixed for the board
    private readonly StartOfFrame _startOfFrame = new(">");
    private readonly EndOfFrame _endOfFrame = new("</");
    private readonly bool _isExtended = false;

    public HardwareId HardwareId { get; } = HardwareId.New;
    public HardwareKind HardwareKind { get; } = HardwareKind.Icb;
    public TimeoutPolicy TimeoutPolicy { get; } = timeoutPolicy;

    public async Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = hardwareClient.ExecuteCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public async Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = hardwareClient.SubmitCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public IAsyncEnumerator<HardwareGatewayEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private CanFrameCommandRequest ConvertToHardwareSpecific(
        HardwareGatewayCommandRequestEnvelope envelope)
    {
        if (!envelope.Address.HasValue)
            throw new ArgumentException($"{nameof(SimulatedIcbAdapter)} requires an address value");
        return new CanFrameCommandRequest(envelope.IdempotencyKey, envelope.CorrelationId, 
            envelope.Address.Value, envelope.Operation, envelope.Payload, envelope.Timeout, _startOfFrame, 
            _endOfFrame, false, _isExtended);
    }
    
    private HardwareCommandResponse ConvertToHardwareSpecific(
        HardwareGatewayCommandResponseEnvelope envelope)
    {
        return new HardwareCommandResponse(envelope.CommandId, envelope.CommandStatus, envelope.Error, envelope.Payload);
    }
    
    private HardwareCommandAccepted ConvertToHardwareSpecific(
        HardwareGatewayCommandAcceptedEnvelope envelope)
    {
        return new HardwareCommandAccepted(envelope.CommandId, envelope.AcceptedAtUtc);
    }
    
    private HardwareEvent ConvertToHardwareSpecific(
        HardwareGatewayEvent gatewayEvent)
    {
        throw new NotImplementedException();
    }

    public HardwareGatewayCommandRequestEnvelope ConvertToGatewaySpecific(
        CanFrameCommandRequest request)
    {
        if (!request.Address.HasValue)
            throw new ArgumentException($"{nameof(SimulatedIcbAdapter)} requires an address value");
        return new HardwareGatewayCommandRequestEnvelope(request.IdempotencyKey, request.CorrelationId,
            request.Address.Value, request.Operation, request.Payload, request.Timeout);
    }
    
    public HardwareGatewayCommandResponseEnvelope ConvertToGatewaySpecific(
        HardwareCommandResponse response)
    {
        return new HardwareGatewayCommandResponseEnvelope(response.CommandId, response.Status, response.Error, response.Payload);
    }
    
    public HardwareGatewayCommandAcceptedEnvelope ConvertToGatewaySpecific(
        HardwareCommandAccepted response)
    {
        return new HardwareGatewayCommandAcceptedEnvelope(response.CommandId, response.AcceptedAtUtc);
    }
    
    public HardwareGatewayEvent ConvertToGatewaySpecific(
        HardwareEvent hardwareEvent)
    {
        throw new NotImplementedException();
    }
}