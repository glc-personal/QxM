namespace Qx.Domain.Labware.Policies;

/// <summary>
/// Seal policy for labware
/// </summary>
/// <param name="isSealed">Boolean value of the labware is sealed (default: false)</param>
public sealed class SealPolicy(bool isSealed=false)
{
    public bool IsSealed => isSealed;
}