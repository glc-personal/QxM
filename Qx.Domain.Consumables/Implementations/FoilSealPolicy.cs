namespace Qx.Domain.Consumables.Implementations;

public sealed class FoilSealPolicy(bool isFoilSealed)
{
    public bool IsFoilSealed { get; } = isFoilSealed;
}