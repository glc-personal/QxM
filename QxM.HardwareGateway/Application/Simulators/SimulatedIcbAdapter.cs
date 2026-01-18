using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application.Simulators;

public sealed class SimulatedIcbAdapter : IHardwareAdapter
{
    // TODO: Make these StartOfFrame, EndOfFrame, and IsExtended configurable since they are fixed for the board
    private IHardwareClient<CanFrameCommandRequest> _hardwareClient;
    private readonly StartOfFrame _startOfFrame;
    private readonly EndOfFrame _endOfFrame;
    private readonly bool _isExtended;

    public SimulatedIcbAdapter(IHardwareClient<CanFrameCommandRequest> hardwareClient)
    {
        HardwareId = HardwareId.New;
        _hardwareClient = hardwareClient;
        _startOfFrame = new StartOfFrame(">");
        _endOfFrame = new EndOfFrame("</");
        _isExtended = false;
    }

    public HardwareId HardwareId { get; } 
    public HardwareKind HardwareKind => HardwareKind.Icb;

    public async Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = _hardwareClient.ExecuteCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public async Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = _hardwareClient.SubmitCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public async IAsyncEnumerator<HardwareGatewayEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var hardwareEvent in _hardwareClient.SubscribeAsync(cancellationToken).ConfigureAwait(false))
            yield return ConvertToGatewaySpecific(hardwareEvent);
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
    
    private HardwareGatewayCommandResponseEnvelope ConvertToGatewaySpecific(
        HardwareCommandResponse response)
    {
        return new HardwareGatewayCommandResponseEnvelope(response.CommandId, response.Status, response.Error, response.Payload);
    }

    private HardwareGatewayCommandAcceptedEnvelope ConvertToGatewaySpecific(
        HardwareCommandAccepted response)
    {
        return new HardwareGatewayCommandAcceptedEnvelope(response.CommandId, response.AcceptedAtUtc);
    }

    private HardwareGatewayEvent ConvertToGatewaySpecific(
        HardwareEvent hardwareEvent)
    {
        return new HardwareGatewayEvent(hardwareEvent.TimestampUtc, hardwareEvent.HardwareId,
            hardwareEvent.HardwareKind,
            hardwareEvent.CorrelationId, hardwareEvent.Address);
    }
}