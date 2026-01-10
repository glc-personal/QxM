namespace QxM.HardwareGateway.Core;

public interface IHardwareGateway
{
    /// <summary>
    /// Connect to the Instrument Control Board CAN bus
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConnectAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Disconnect from the Instrument Control Board CAN bus
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send a command request to the Instrument Control Board CAN bus
    /// </summary>
    /// <param name="request"><see cref="SendCommandRequest"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SendCommandResponse> SendCommandAsync(SendCommandRequest request, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check on the command request which has been sent
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CommandEvent> CheckCommandAsync(CheckCommandRequest request, 
        CancellationToken cancellationToken = default);
}