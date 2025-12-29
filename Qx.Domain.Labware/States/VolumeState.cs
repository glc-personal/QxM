using Qx.Domain.Labware.Models;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Labware.States;

// Domain Invariants Note: volume state is not coupled to well capacity (loosely coupled), so no rules on adding above capacity here
public sealed class VolumeState
{
    private readonly Dictionary<WellAddress, Volume> _volumesUl;

    public VolumeState(IEnumerable<WellAddress> wellAddresses)
    {
        _volumesUl = wellAddresses.ToDictionary(w => w, w => new Volume(0D, VolumeUnits.Ul));
    }
    
    public Volume GetVolume(WellAddress wellAddress) 
        => _volumesUl.TryGetValue(wellAddress, out Volume volume) 
            ? volume : throw new KeyNotFoundException($"Cannot find volume for well {wellAddress}, address not found");

    public void SetVolume(WellAddress wellAddress, Volume volume)
    {
        EnforceDomainInvariantWellAddress(wellAddress);
        if (volume.Value < 0.0D) throw new ArgumentOutOfRangeException($"{nameof(Volume)} ({volume}) cannot be negative");
        _volumesUl[wellAddress] = volume;
    }

    public void AddVolume(WellAddress wellAddress, Volume volume)
    {
        EnforceDomainInvariantWellAddress(wellAddress);
        EnforceDomainInvariantPositiveVolume(volume);
        _volumesUl[wellAddress] += volume;
    }

    public void RemoveVolume(WellAddress wellAddress, Volume volume)
    {
        EnforceDomainInvariantWellAddress(wellAddress);
        EnforceDomainInvariantPositiveVolume(volume);
        if (_volumesUl[wellAddress].Value - volume.Value <= 0.0D)
            _volumesUl[wellAddress].Zero();
        else
            _volumesUl[wellAddress] -= volume;
    }

    private void EnforceDomainInvariantWellAddress(WellAddress wellAddress)
    {
        if (!_volumesUl.ContainsKey(wellAddress)) 
            throw new ArgumentException($"Cannot set volume for well {wellAddress}, address not found");
    }

    private void EnforceDomainInvariantPositiveVolume(Volume volume)
    {
        if (volume.Value <= 0.0D) 
            throw new ArgumentOutOfRangeException($"{nameof(Volume)} ({volume}) must be a positive");
    }
}