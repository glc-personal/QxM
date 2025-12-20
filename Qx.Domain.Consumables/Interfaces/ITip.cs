namespace Qx.Domain.Consumables.Interfaces;

public interface ITip : IConsumable, IVolumeContainer
{
    ITip ShallowCopy();
}