using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Exceptions;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Well for holding a liquid
/// </summary>
public class Well
{
    private Volume _volume;

    public Well(WellTypes type, WellAddress address, Volume volume, WellCapacity capacity)
    {
        if (volume > capacity.Maximum)
            throw new MaximumVolumeExceededException(volume, capacity.Maximum);
        
        Type = type;
        Address = address;
        _volume = volume;
        Capacity = capacity;
    }
    
    /// <summary>
    /// Well type
    /// </summary>
    public WellTypes Type { get; }

    /// <summary>
    /// Well address
    /// </summary>
    public WellAddress Address { get; }

    /// <summary>
    /// Current well volume
    /// </summary>
    public Volume Volume => _volume;
    
    /// <summary>
    /// Well volume capacity
    /// </summary>
    public WellCapacity Capacity { get; }
    
    /// <summary>
    /// Row index of the well
    /// </summary>
    public int RowIndex => Address.Row;
    
    /// <summary>
    /// Column index of the well
    /// </summary>
    public int ColumnIndex => Address.Column;

    /// <summary>
    /// Add volume to the well
    /// </summary>
    /// <param name="volume">Volume to be added to the well</param>
    public void AddVolume(Volume volume)
    {
        var combinedVolume = _volume + volume;
        if (combinedVolume > Capacity.Maximum)
            throw new MaximumVolumeExceededException(combinedVolume, Capacity.Maximum);
        _volume = combinedVolume;
    }

    /// <summary>
    /// Remove a volume from the well
    /// </summary>
    /// <param name="volume">Volume to be removed from the well</param>
    public void RemoveVolume(Volume volume)
    {
        if (volume > _volume)
            _volume -= _volume; // set volume to 0, don't want a negative volume
        else
            _volume -= volume;
    }
}