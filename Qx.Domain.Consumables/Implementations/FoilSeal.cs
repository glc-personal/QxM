using Qx.Domain.Consumables.Enums;

namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Foil seal for a collection of well columns
/// </summary>
public sealed class FoilSeal
{
    private readonly int _numberOfWellColumns;
    private IList<FoilSealStates> _wellColumnSeals;

    /// <summary>
    /// Constructor for FoilSeal
    /// </summary>
    /// <param name="numberOfWellColumns">Number of well columns to be covered by the foil seal</param>
    public FoilSeal(int numberOfWellColumns)
    {
        _numberOfWellColumns = numberOfWellColumns;
        SealAllWells();
    }
    
    /// <summary>
    /// List of well column seals
    /// </summary>
    public IList<FoilSealStates> WellColumnSeals => _wellColumnSeals;

    /// <summary>
    /// Pierce the well column seals provided the column index
    /// </summary>
    /// <param name="columnIndex">Column index for the well column to be pierced</param>
    public void PierceWellColumnSeals(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= _numberOfWellColumns)
            throw new IndexOutOfRangeException($"Column index ({columnIndex}) is out of range ({0} <= index < {_numberOfWellColumns}).");
        if (!IsWellColumnSealed(columnIndex))
            return;
        _wellColumnSeals[columnIndex] = FoilSealStates.Pierced;
    }
    
    /// <summary>
    /// Checks if a well column is sealed
    /// </summary>
    /// <param name="columnIndex">Well column to be checked</param>
    /// <returns></returns>
    public bool IsWellColumnSealed(int columnIndex)
    {
        return _wellColumnSeals[columnIndex] == FoilSealStates.Sealed;
    }

    private void SealAllWells()
    {
        _wellColumnSeals = new List<FoilSealStates>();
        for (int i = 0; _numberOfWellColumns > i; i++)
            _wellColumnSeals.Add(FoilSealStates.Sealed);
    }
}