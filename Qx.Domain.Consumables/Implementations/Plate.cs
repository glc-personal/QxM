using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Liquids.Records;
using Qx.Domain.Locations.Interfaces;

namespace Qx.Domain.Consumables.Implementations;

public sealed class Plate(
    int id,
    ConsumableTypes type,
    ReusePolicy reusePolicy,
    ILocation? location,
    IReadOnlyList<WellColumn> wellColumns)
    : IVolumeContainer
{
    public int Id { get; } = id;
    public ConsumableTypes Type { get; } = type;
    public ConsumableStates State { get; } = ConsumableStates.Available;
    public ReusePolicy ReusePolicy { get; } = reusePolicy;
    public int Uses { get; } = 0;
    public ILocation Location { get; } = location;
    public IReadOnlyList<WellColumn> WellColumns { get; } = wellColumns;
   
    /// <inheritdoc />
    public void AddVolume(Volume[] volumes, int columnIndex)
    {
        var wellColumn = ValidateColumnIndex(columnIndex);
        wellColumn.AddVolume(volumes);
    }

    /// <inheritdoc />
    public void RemoveVolume(Volume[] volumes, int columnIndex)
    {
        var wellColumn = ValidateColumnIndex(columnIndex);
        wellColumn.RemoveVolume(volumes);
    }

    private WellColumn ValidateColumnIndex(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= WellColumns.Count)
            throw new IndexOutOfRangeException($"Column index {columnIndex} is out of range (0 < index < {WellColumns.Count}).");
        return WellColumns[columnIndex];
    }
}