using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IVolumeContainer : IConsumable
{
    IReadOnlyList<WellColumn> WellColumns { get; }
    
    /// <summary>
    /// Add volume to the container to all wells in a particular column
    /// </summary>
    /// <param name="volumes">Volumes to be added</param>
    /// <param name="columnIndex">Column index of the wells the volume is added to</param>
    public void AddVolume(Volume[] volumes, int columnIndex);
    
    /// <summary>
    /// Remove volume from the container from all the wells in a particular column
    /// </summary>
    /// <param name="volumes">Volumes to be removed</param>
    /// <param name="columnIndex">Column index of the wells the volume will be removed from</param>
    public void RemoveVolume(Volume[] volumes, int columnIndex);
}