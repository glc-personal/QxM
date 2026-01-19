using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application.Simulators;

public class SimulatedHardwareAdapterBase<TRequest>(IHardwareClient<TRequest> hardwareClient) : IHardwareAdapter
    where TRequest : HardwareCommandRequest
{
    public HardwareId HardwareId { get; } = hardwareClient.HardwareId;
    public HardwareKind HardwareKind { get; } = hardwareClient.HardwareKind;

    public virtual async Task<HardwareGatewayCommandResponseEnvelope> ExecuteCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = hardwareClient.ExecuteCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public virtual async Task<HardwareGatewayCommandAcceptedEnvelope> SubmitCommandAsync(HardwareGatewayCommandRequestEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var hardwareCommandRequest = ConvertToHardwareSpecific(envelope);
        var task = hardwareClient.SubmitCommandAsync(hardwareCommandRequest, cancellationToken);
        var response = await task.ConfigureAwait(false);
        var responseEnvelope =  ConvertToGatewaySpecific(response);
        return responseEnvelope;
    }

    public virtual async IAsyncEnumerator<HardwareGatewayEventEnvelope> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var hardwareEvent in hardwareClient.SubscribeAsync(cancellationToken).ConfigureAwait(false))
            yield return ConvertToGatewaySpecific(hardwareEvent);
    }

    protected virtual TRequest ConvertToHardwareSpecific(
        HardwareGatewayCommandRequestEnvelope envelope)
    {
        throw new NotImplementedException();
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

    private HardwareGatewayEventEnvelope ConvertToGatewaySpecific(
        HardwareEvent hardwareEvent)
    {
        return new HardwareGatewayEventEnvelope(hardwareEvent.TimestampUtc, hardwareEvent.HardwareId,
            hardwareEvent.HardwareKind,
            hardwareEvent.CorrelationId, hardwareEvent.Address);
    }

    public async ValueTask DisposeAsync()
    {
        await hardwareClient.DisposeAsync();
    }
}