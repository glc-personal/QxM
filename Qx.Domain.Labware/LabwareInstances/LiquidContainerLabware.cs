using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Labware.Models;
using Qx.Domain.Labware.States;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Labware.LabwareInstances;

public sealed class LiquidContainerLabware : Labware
{
    private readonly LiquidContainerModel _liquidContainerModel;
    private readonly Dictionary<int, SealState> _sealStates = new Dictionary<int, SealState>();
    private readonly int _rowCount;
    private readonly int _columnCount;
    private VolumeState _volumeState;
    
    private LiquidContainerLabware(LabwareDefinition definition, Guid? id = null) : base(definition, id)
    {
        _liquidContainerModel = definition.LiquidContainerModel 
                                ?? throw new NullReferenceException($"{nameof(definition.LiquidContainerModel)} must not be null for a {nameof(LiquidContainerLabware)}.");
        var columnIndexes = new List<int>();
        foreach (var column in _liquidContainerModel.Columns)
        {
            columnIndexes.Add(column.ColumnIndex);
            _sealStates.Add(column.ColumnIndex, column.SealPolicy.IsSealed ? SealState.Sealed() : SealState.Unsealed());
        }
        _rowCount = _liquidContainerModel.RowCount;
        _columnCount = _liquidContainerModel.ColumnCount;
        var wellAddresses = new List<WellAddress>();
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            foreach (var columnIndex in columnIndexes)
                wellAddresses.Add(new WellAddress(rowIndex, columnIndex));
        }
        _volumeState = new VolumeState(wellAddresses);
    }
    
    public static LiquidContainerLabware Create(LabwareDefinition definition, Guid? id = null)
    {
        return new LiquidContainerLabware(definition,id); 
    }
    
    /// <summary>
    /// Get the capacity of a column of wells
    /// </summary>
    /// <param name="columnIndex">Column index for wells of interest</param>
    /// <returns></returns>
    public WellCapacity GetWellColumnCapacity(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        var wellCapacity = _liquidContainerModel.Columns[columnIndex].WellDefinition.Capacity;
        return wellCapacity;
    }

    /// <summary>
    /// Get the volume of a well
    /// </summary>
    /// <param name="wellAddress"></param>
    /// <returns></returns>
    public Volume GetWellVolume(WellAddress wellAddress)
    {
        EnforceValidColumnIndex(wellAddress.ColumnIndex);
        return _volumeState.GetVolume(wellAddress);
    }

    /// <summary>
    /// Get the seal state for a column by index
    /// </summary>
    /// <param name="columnIndex">Column index of the column of interest</param>
    /// <returns></returns>
    public SealState GetColumnSealState(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        return _sealStates[columnIndex];
    }

    /// <summary>
    /// Pierce seal on column by column index
    /// </summary>
    /// <param name="columnIndex">Column index of column to be pierced</param>
    public void PierceColumnSeal(int columnIndex)
    {
        EnforceValidColumnIndex(columnIndex);
        _sealStates[columnIndex] = SealState.Pierced(DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Add volume to the wells of a column
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <param name="volumesByRow">Volume to add per row</param>
    public void DispenseToColumn(int columnIndex, IReadOnlyList<Volume> volumesByRow)
    {
        EnforceValidColumnIndex(columnIndex);
        EnforceValidVolumes(volumesByRow, false);
        EnforceDispenseOnSealedColumnFails(columnIndex);
        var wellCapacity = GetWellColumnCapacity(columnIndex);
        var futures = new List<Volume>();
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var wellAddress = new WellAddress(rowIndex, columnIndex);
            futures.Add(_volumeState.GetVolume(wellAddress) + volumesByRow[rowIndex]);
        }
        EnforceTotalVolumeFromWellCapacity(wellCapacity, columnIndex, futures);
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var wellAddress = new WellAddress(rowIndex, columnIndex);
            _volumeState.AddVolume(wellAddress, volumesByRow[rowIndex]);
        }
    }

    /// <summary>
    /// Remove volume from the wells of a column
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <param name="volumesByRow">Volume to remove per row</param>
    public void AspirateFromColumn(int columnIndex, IReadOnlyList<Volume> volumesByRow)
    {
        EnforceValidColumnIndex(columnIndex);
        EnforceAspirationOnSealedColumnFails(columnIndex);
        EnforceValidVolumes(volumesByRow, false);
        EnforceWellVolumesAreSufficientForAspirate(columnIndex, volumesByRow);
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var wellAddress = new WellAddress(rowIndex, columnIndex);
            _volumeState.RemoveVolume(wellAddress, volumesByRow[rowIndex]);
        }
    }
    
    public void SetColumnVolume(int columnIndex, IReadOnlyList<Volume> volumesByRow)
    {
        EnforceValidColumnIndex(columnIndex);
        EnforceValidVolumes(volumesByRow, false);
        var wellCapacity = GetWellColumnCapacity(columnIndex);
        EnforceTotalVolumeFromWellCapacity(wellCapacity, columnIndex, volumesByRow);
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var wellAddress = new WellAddress(rowIndex, columnIndex);
            _volumeState.SetVolume(wellAddress, volumesByRow[rowIndex]);
        }
    }

    private void EnforceTotalVolumeFromWellCapacity(WellCapacity capacityOfColumn, int columnIndex, IReadOnlyList<Volume> volumesByRow)
    {
        var rowIndexesWithIssues = new List<int>();
        var invalidVolumes = new List<Volume>();
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var volume = volumesByRow[rowIndex];
            if (volume > capacityOfColumn.MaxVolume)
            {
                rowIndexesWithIssues.Add(rowIndex);
                invalidVolumes.Add(volume);
            }
        }

        if (rowIndexesWithIssues.Count > 0)
        {
            var invalidRowsString = string.Join(", ", rowIndexesWithIssues);
            var invalidVolumesString = string.Join(", ", invalidVolumes);
            throw new InvalidOperationException($"Invalid volumes ({invalidVolumesString}) in row(s) {invalidRowsString} of column {columnIndex}. Volumes cannot exceed capacity of well ({capacityOfColumn.MaxVolume}).");
        }
    }

    private void EnforceWellVolumesAreSufficientForAspirate(int columnIndex, IReadOnlyList<Volume> volumesByRow)
    {
        var rowIndexesWithIssues = new List<int>();
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var volume = volumesByRow[rowIndex];
            if (volume > GetWellVolume(new WellAddress(rowIndex, columnIndex)))
            {
                rowIndexesWithIssues.Add(rowIndex);
            }
        }

        if (rowIndexesWithIssues.Count > 0)
        {
            var invalidRowsString = string.Join(", ", rowIndexesWithIssues);
            // TODO: Don't ruin it for all the rows, only for the failed ones
            throw new ArgumentException($"Insufficient volumes for aspiration in row(s) {invalidRowsString} of column {columnIndex}.");
        }
    }

    private void EnforceValidColumnIndex(int columnIndex)
    {
        if (!_sealStates.ContainsKey(columnIndex))
            throw new KeyNotFoundException($"Invalid column index {columnIndex}, cannot find seal state");
    }

    private void EnforceValidVolumes(IReadOnlyList<Volume> volumesByRow, bool allowZeroVolume)
    {
        if (volumesByRow.Count != _rowCount)
            throw new ArgumentException(
                $"Number of volumes ({volumesByRow.Count}) must be equal to row count {_rowCount}");
        for (int rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            var volume = volumesByRow[rowIndex];
            if (volume.Value < 0D) throw new ArgumentException($"Volume ({volume}) for row ({rowIndex}) cannot be negative");
            if (!allowZeroVolume && volume.Value == 0D) throw new ArgumentException($"Volume ({volume}) for row ({rowIndex}) cannot be zero");
        }
    }

    private void EnforceAspirationOnSealedColumnFails(int columnIndex)
    {
        if (_sealStates[columnIndex].Kind == SealStateKind.Sealed) 
            throw new InvalidOperationException($"Cannot aspirate from a {_sealStates[columnIndex].Kind} well column ({columnIndex})");
    }

    private void EnforceDispenseOnSealedColumnFails(int columnIndex)
    {
        if (_sealStates[columnIndex].Kind == SealStateKind.Sealed) 
            throw new InvalidOperationException($"Cannot dispense to {_sealStates[columnIndex].Kind} well column ({columnIndex})");
    }
}