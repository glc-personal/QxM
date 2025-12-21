using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Liquids.Records;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

public sealed class Plate : IPlate
{
    private int _uses = 0;
    private ConsumableStates _state = ConsumableStates.Available;
    private readonly FoilSeal _foilSeal;

    public Plate(string name, ReusePolicy reusePolicy, Location? location,
        IReadOnlyList<WellColumn> wellColumns, FoilSealPolicy foilSealPolicy)
    {
        UniqueIdentifier = Guid.NewGuid();
        Name = name;
        Type = ConsumableTypes.NinetySixWellPlate; // stinky (use a factory to build a plate)
        State = _state;
        ReusePolicy = reusePolicy;
        Uses = 0;
        Location = location;
        WellColumns = wellColumns;
        FoilSealPolicy = foilSealPolicy;
        if (FoilSealPolicy.IsFoilSealed)
            _foilSeal = new FoilSeal(WellColumns.Count);
    }
    
    public Guid UniqueIdentifier { get; }
    public string Name { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State { get; }
    public ReusePolicy ReusePolicy { get; }
    public int Uses { get; }
    public Location Location { get; }
    public IReadOnlyList<WellColumn> WellColumns { get; }
    public FoilSealPolicy FoilSealPolicy { get; }

    /// <inheritdoc />
    public void AddVolume(Volume[] volumes, int columnIndex)
    {
        ValidatePlateStateForVolumeChange(columnIndex, nameof(AddVolume));
        WellColumns[columnIndex].AddVolume(volumes);
    }

    /// <inheritdoc />
    public void RemoveVolume(Volume[] volumes, int columnIndex)
    {
        ValidatePlateStateForVolumeChange(columnIndex, nameof(RemoveVolume));
        WellColumns[columnIndex].RemoveVolume(volumes);
    }

    /// <inheritdoc />
    public void PierceFoilSeal(int columnIndex)
    {
        if (FoilSealPolicy.IsFoilSealed)
            _foilSeal.PierceWellColumnSeals(columnIndex);
    }

    private void CheckColumnIndex(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= WellColumns.Count)
            throw new IndexOutOfRangeException($"Column index {columnIndex} is out of range (0 < index < {WellColumns.Count}).");
    }

    private void CheckFoilSeal(int columnIndex, string operation)
    {
        if (FoilSealPolicy.IsFoilSealed && _foilSeal.IsWellColumnSealed(columnIndex)) 
            throw new PlateFoilSealException(this, operation, columnIndex);
    }

    private void CheckReuse()
    {
        if (!ReusePolicy.CanUse(_uses))
            throw new OutOfUsesException(_uses, ReusePolicy.MaxUses.Value);
    }

    private void ValidatePlateStateForVolumeChange(int columnIndex, string operation)
    {
        if (State != ConsumableStates.Available)
            throw new InvalidConsumableStateException(State, ConsumableStates.Available);
        CheckReuse();
        CheckColumnIndex(columnIndex);
        CheckFoilSeal(columnIndex, operation);
    }
}