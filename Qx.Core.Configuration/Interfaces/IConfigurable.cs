using System.Text.Json.Nodes;

namespace Qx.Core.Configuration.Interfaces;

public interface IConfigurable
{
    /// <summary>
    /// Configure based on a file
    /// </summary>
    /// <param name="filePath">file path to the config file</param>
    void Configure(string filePath);
    
    /// <summary>
    /// Configure based on a json object
    /// </summary>
    /// <param name="jsonObject">json object to be used</param>
    void Configure(JsonObject jsonObject);
    
    /// <summary>
    /// Event for config change tracking
    /// </summary>
    event EventHandler ConfigurationChanged;
}