using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IVolumeContainer
{
    /// <summary>
    /// Current volume of the container
    /// </summary>
    Volume Volume { get; }
    
    /// <summary>
    /// Volume capacity of the container
    /// </summary>
    VolumeContainerCapacity Capacity { get; }
    
    /// <summary>
    /// Add volume to the container
    /// </summary>
    /// <param name="volume">Volume to be added</param>
    void AddVolume(Volume volume);
    
    /// <summary>
    /// Remove volume from the container
    /// </summary>
    /// <param name="volume">Volume to be removed</param>
    void RemoveVolume(Volume volume);
}