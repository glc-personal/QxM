namespace Qx.Domain.Consumables.Implementations;

public sealed class SealPolicy(bool isSealed)
{
    public bool IsSealed { get; } = isSealed;
}