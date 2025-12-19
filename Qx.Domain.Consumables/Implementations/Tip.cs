using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Liquids.Exceptions;
using Qx.Domain.Liquids.Records;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Consumables.Implementations;

public sealed class Tip : ITip
{
    private int _uses = 0;
    private ConsumableStates _state = ConsumableStates.Available;
    private Volume _volume;
    
    public Tip(int id, ReusePolicy reusePolicy, Location location, VolumeContainerCapacity capacity)
    {
        Id = id;
        ReusePolicy = reusePolicy;
        Location = location;
        Capacity = capacity;
        Name = $"{Capacity.ToString()} tip";
        UniqueIdentifier = Guid.NewGuid();
        Type = ConsumableTypes.TipRack;
        Uses = _uses;
        _volume = new Volume(0.0, capacity.Maximum.Units);
    }
    public int Id { get; }
    public string Name { get; }
    public Guid UniqueIdentifier { get; }
    public ConsumableTypes Type { get; }
    public ConsumableStates State => _state;
    public ReusePolicy ReusePolicy { get; }
    public int Uses { get; }
    public Location Location { get; }
    public Volume Volume => _volume;
    public VolumeContainerCapacity Capacity { get; }
    
    public void AddVolume(Volume volume)
    {
        var futureVolume = volume + Volume;
        CheckVolumeChange(futureVolume);
        // TODO: Checking reuse and state is redundant here
        CheckReuse();
        CheckState();
        _volume = futureVolume;
        _uses++;
        GetState();
    }

    public void RemoveVolume(Volume volume)
    {
        var futureVolume = Volume - volume;
        // TODO: Checking reuse and state is redundant here
        CheckReuse();
        CheckState();
        _volume = futureVolume;
        _uses++;
        GetState();
    }

    private void CheckVolumeChange(Volume futureVolume)
    {
        if (futureVolume > Capacity.Maximum)
            throw new MaximumVolumeExceededException(futureVolume, Capacity.Maximum);
    }

    private void CheckReuse()
    {
        if (!ReusePolicy.CanUse(_uses))
            throw new OutOfUsesException(_uses, ReusePolicy.MaxUses.Value);
    }

    private void CheckState()
    {
        if (_state != ConsumableStates.Available)
            throw new InvalidConsumableStateException(_state, ConsumableStates.Available);
    }

    private void GetState()
    {
        if (_uses >= ReusePolicy.MaxUses)
            _state = ConsumableStates.Consumed;
    }
}