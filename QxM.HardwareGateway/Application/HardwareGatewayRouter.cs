using System.Collections.ObjectModel;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;

namespace QxM.HardwareGateway.Application;

public sealed class HardwareGatewayRouter : IAsyncDisposable
{
    private readonly ReadOnlyDictionary<HardwareId, IHardwareAdapter> _hardwareAdapters;

    public HardwareGatewayRouter(IEnumerable<IHardwareAdapter> hardwareAdapters)
    {
        _hardwareAdapters = new ReadOnlyDictionary<HardwareId, IHardwareAdapter>(
            hardwareAdapters.ToDictionary(hd => hd.HardwareId, hd => hd));
    }

    public async Task<HardwareGatewayCommandResponseEnvelope> RouteExecuteCommandAsync(
        HardwareGatewayRoutingScheme scheme, HardwareGatewayCommandRequestEnvelope envelope, 
        CancellationToken cancellationToken = default)
    {
        EnforceValidHardware(scheme.HardwareId, scheme.HardwareKind);
        var hardwareAdapter = _hardwareAdapters[scheme.HardwareId];
        var responseEnvelope = await hardwareAdapter.ExecuteCommandAsync(envelope, cancellationToken)
            .ConfigureAwait(false);
        return responseEnvelope;
    }

    public async Task<HardwareGatewayCommandAcceptedEnvelope> RouteAcceptCommandAsync(
        HardwareGatewayRoutingScheme scheme, HardwareGatewayCommandRequestEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        EnforceValidHardware(scheme.HardwareId, scheme.HardwareKind);
        var hardwareAdapter = _hardwareAdapters[scheme.HardwareId];
        var acceptedEnvelope = await hardwareAdapter.SubmitCommandAsync(envelope, cancellationToken)
            .ConfigureAwait(false);
        return acceptedEnvelope;
    }

    private void EnforceValidHardware(HardwareId hardwareId, HardwareKind hardwareKind)
    {
        if (!_hardwareAdapters.ContainsKey(hardwareId))
            throw new ArgumentException($"{nameof(HardwareGatewayRouter)} does not contain a {hardwareKind} with ID {hardwareId}");
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var hardwareAdapter in _hardwareAdapters.Values)
            await hardwareAdapter.DisposeAsync().ConfigureAwait(false);
    }
}