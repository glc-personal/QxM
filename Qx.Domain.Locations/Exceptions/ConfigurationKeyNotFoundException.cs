namespace Qx.Domain.Locations.Exceptions;

public class ConfigurationKeyNotFoundException(string nameof, string key) : KeyNotFoundException($"The configuration ({nameof}) key '{key}' was not found.")
{
    
}