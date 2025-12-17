using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Implementations;

public sealed class WellColumn(IReadOnlyList<Well> wells)
{
    public IReadOnlyList<Well> Wells { get; } = wells;

    /// <summary>
    /// Add volume to each well in the well column
    /// </summary>
    /// <param name="volumes">Volumes to be added to each well</param>
    public void AddVolume(Volume[] volumes)
    {
        if (volumes is null) throw new ArgumentNullException(nameof(volumes));
        if (volumes.Length != wells.Count) 
            throw new ArgumentException($"The number of volumes ({volumes.Length}) does not match the number of wells ({wells.Count}).");
        for (int i = 0; i < volumes.Length; i++)
            wells[i].AddVolume(volumes[i]);
    }

    /// <summary>
    /// Remove volume from each well in the well column
    /// </summary>
    /// <param name="volumes">Volumes to be removed from the wells</param>
    public void RemoveVolume(Volume[] volumes)
    {
        if (volumes is null) throw new ArgumentNullException(nameof(volumes));
        if (volumes.Length != wells.Count) 
            throw new ArgumentException($"The number of volumes ({volumes.Length}) does not match the number of wells ({wells.Count}).");
        for (int i = 0; i < volumes.Length; i++)
            wells[i].RemoveVolume(volumes[i]);
    }
}