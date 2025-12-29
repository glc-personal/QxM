using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Exceptions;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Consumables.Implementations;

public class ConsumableColumn : IVolumeContainer
{
    private IList<Volume> _volumes;
    private IList<VolumeContainerCapacity> _capacities;
    private int _uses;
    public bool _isSealed;
    
    public ConsumableColumn(IList<VolumeContainerCapacity> capacities, ReusePolicy reusePolicy, SealPolicy sealPolicy)
    {
        _volumes = new List<Volume>(capacities.Count);
        _capacities = capacities;
        ReusePolicy = reusePolicy;
        SealPolicy = sealPolicy;
        _isSealed = sealPolicy.IsSealed;
        _uses = 0;
        Id = Guid.NewGuid();
    }

    public ReusePolicy ReusePolicy { get; }
    public SealPolicy SealPolicy { get; }
    public IList<Volume> Volumes => _volumes;
    public IList<VolumeContainerCapacity> Capacities => _capacities;
    public Guid Id { get; }
    
    public void AddVolume(IList<Volume> volumes)
    {
        ValidateVolumeChange();
        
        for (int i = 0; i < volumes.Count; i++)
        {
            var futureVolume = _volumes[i] + volumes[i];
            if (futureVolume > _capacities[i].Maximum)
                throw new CapacityExceededException(futureVolume, _capacities[i].Maximum);
            _volumes[i] = futureVolume;
        }
        _uses++;
    }

    public void RemoveVolume(IList<Volume> volumes)
    {
        ValidateVolumeChange();
        
        for (int i = 0; i < volumes.Count; i++)
        {
            var futureVolume = _volumes[i] - volumes[i];
            if (futureVolume.Value < 0)
                throw new InvalidVolumeException(futureVolume);
            _volumes[i] = futureVolume;
        }
    }

    private void ValidateVolumeChange()
    {
        if (_isSealed) throw new SealException();
        if (!ReusePolicy.CanUse(_uses)) throw new OutOfUsesException(_uses, ReusePolicy.MaxUses.Value);
    }
}