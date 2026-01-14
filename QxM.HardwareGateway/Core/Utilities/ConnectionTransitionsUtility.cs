using System.Threading.Channels;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.State;

namespace QxM.HardwareGateway.Core.Utilities;

public static class ConnectionTransitionsUtility
{
    /// <summary>
    /// Build the valid transitions for <see cref="ConnectionState" and <see cref="ConnectionTrigger"/>/>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Transition<ConnectionState, ConnectionTrigger>> BuildTransitions()
    {
        return
        [
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Disconnected, ConnectionState.Connecting,
                ConnectionTrigger.Connect),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Disconnected, ConnectionState.Faulted,
                ConnectionTrigger.Fault),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Connecting, ConnectionState.Connected,
                ConnectionTrigger.ConnectSucceeded),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Connecting, ConnectionState.Faulted,
                ConnectionTrigger.ConnectFailed),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Connecting, ConnectionState.Faulted,
                ConnectionTrigger.Fault),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Connected, ConnectionState.Disconnecting,
                ConnectionTrigger.Disconnect),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Connected, ConnectionState.Faulted,
                ConnectionTrigger.Fault),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Disconnecting, ConnectionState.Disconnected,
                ConnectionTrigger.DisconnectSucceeded),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Disconnecting, ConnectionState.Faulted,
                ConnectionTrigger.DisconnectFailed),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Disconnecting, ConnectionState.Faulted,
                ConnectionTrigger.Fault),
            new Transition<ConnectionState, ConnectionTrigger>(ConnectionState.Faulted, ConnectionState.Disconnecting,
                ConnectionTrigger.ResetFault)
        ];
    }
}