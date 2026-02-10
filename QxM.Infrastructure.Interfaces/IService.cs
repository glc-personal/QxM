namespace QxM.Infrastructure.Interfaces;

public interface IService
{
    /// <summary>
    /// Initialize the service by configuring it with an associated configuration
    /// and set up any other necessary initialization processes (e.g. State Machine,
    /// Data Management, etc.)
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Activate the service (e.g. connect infrastructure, home motors, etc.)
    /// </summary>
    void Activate();
    
    /// <summary>
    /// Deactivate the service
    /// </summary>
    void Deactivate();
}