using System.Collections;

namespace Qx.Core.Configuration.Interfaces;

public interface IConfiguration<out TModel, TKey> : IConfigurable where TModel : IDictionary
{
    TModel Model { get; }
    
    /// <summary>
    /// Get the value of the provided path into the config model
    /// </summary>
    /// <param name="keys">ordered list of keys to define a path in the config model</param>
    /// <param name="value">value of interest</param>
    void GetValue(List<TKey> keys, ref object? value);
}