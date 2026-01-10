using System.IO.Ports;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;

namespace QxM.HardwareGateway.Infrastructure;

public sealed class GatewayCommunication : IGatewayCommunication
{
    private GatewayCommunicationOptions _options = null!;
    private SerialPort _serialPort = null!;

    private GatewayCommunication(GatewayCommunicationOptions options)
    {
        Configure(options);
    }

    public static IGatewayCommunication Create(GatewayCommunicationOptions options)
    {
        return new GatewayCommunication(options);
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_options == null) 
            throw new InvalidOperationException($"{nameof(GatewayCommunication)} cannot connect, it has not been configured.");
        try
        {
            _serialPort.Open();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"{nameof(GatewayCommunication)} cannot connect to {_options.ComPort}.", ex);
        }
        return Task.CompletedTask;
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        _serialPort.Close();
        return Task.CompletedTask;
    }

    public Task Write(CanFrame frame, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CanFrame> ReadAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private void Configure(GatewayCommunicationOptions options)
    {
        _options = options;
        _serialPort = new SerialPort(options.ComPort, options.BaudRate);
    }
}