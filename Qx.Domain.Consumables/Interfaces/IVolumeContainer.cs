using Qx.Core;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IVolumeContainer : IUniquelyIdentifiable
{
    /// <summary>
    /// Reuse policy of the container
    /// </summary>
    ReusePolicy ReusePolicy { get; }
    
    /// <summary>
    /// Seal policy of the container
    /// </summary>
    SealPolicy SealPolicy { get; }
    
    /// <summary>
    /// Current volume of the container
    /// </summary>
    IList<Volume> Volumes { get; }
    
    /// <summary>
    /// Volume capacity of the container
    /// </summary>
    IList<VolumeContainerCapacity> Capacities { get; }
    
    /// <summary>
    /// Add volume to the container
    /// </summary>
    /// <param name="volume">Volume to be added</param>
    void AddVolume(IList<Volume> volumes);
    
    /// <summary>
    /// Remove volume from the container
    /// </summary>
    /// <param name="volume">Volume to be removed</param>
    void RemoveVolume(IList<Volume> volumes);
}