using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

/// <summary>
/// Simulated communication interface to the Instrument Control Board CAN bus
/// </summary>
public sealed class SimulatedGatewayCommunication : IGatewayCommunication
{
    private CanFrame _canFrame;
    private GatewayCommunicationOptions _options;

    private SimulatedGatewayCommunication(GatewayCommunicationOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Gateway Communication Factory from <see cref="GatewayCommunicationOptions"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IGatewayCommunication Create(GatewayCommunicationOptions options)
    {
        return new SimulatedGatewayCommunication(options);
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Connecting to the {nameof(SimulatedGatewayCommunication)}");
        return Task.CompletedTask;
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Disconnecting from the {nameof(SimulatedGatewayCommunication)}");
        return Task.CompletedTask;
    }

    public Task Write(CanFrame frame, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Writing CAN Frame: {frame}");
        _canFrame = frame;
        return Task.CompletedTask;
    }

    public Task<CanFrame> ReadAsync(CancellationToken cancellationToken = default)
    {
        var canFrame = CanFrame.Create(_canFrame.Id, _canFrame.IsRemoteTransmitRequest, _canFrame.Data);
        Console.WriteLine($"Reading CAN Frame: {canFrame}");
        return Task.FromResult(canFrame);
    }
    
    private void Configure(GatewayCommunicationOptions options)
    {
        _options = options;
        Console.WriteLine($"Configured Gateway Communication Options: {options}");
    }
}