using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Interfaces;

public interface IPlate : IConsumable
{
    /// <summary>
    /// Well columns
    /// </summary>
    IReadOnlyList<WellColumn> WellColumns { get; }
    
    /// <summary>
    /// Foil seal policy for the plate
    /// </summary>
    FoilSealPolicy FoilSealPolicy { get; }
    
    /// <summary>
    /// Add volume to the container to all wells in a particular column
    /// </summary>
    /// <param name="volumes">Volumes to be added</param>
    /// <param name="columnIndex">Column index of the wells the volume is added to</param>
    void AddVolume(Volume[] volumes, int columnIndex);
    
    /// <summary>
    /// Remove volume from the container from all the wells in a particular column
    /// </summary>
    /// <param name="volumes">Volumes to be removed</param>
    /// <param name="columnIndex">Column index of the wells the volume will be removed from</param>
    void RemoveVolume(Volume[] volumes, int columnIndex);
    
    /// <summary>
    /// Pierce foil seal for a particular column
    /// </summary>
    /// <param name="columnIndex">Column of the plate to be foil pierced</param>
    void PierceFoilSeal(int columnIndex);
}