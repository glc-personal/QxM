namespace Qx.Domain.Consumables.Implementations;

/// <summary>
/// Reuse policy for consumables
/// </summary>
public sealed class ReusePolicy
{
    public bool IsReusable { get; }
    public int? MaxUses { get; } // null -> no limit
    
    public ReusePolicy(bool isReusable, int? maxUses = null)
    {
        if (!isReusable && maxUses != null)
            throw new ArgumentException("Max uses cannot be specified if not reusable.");
        
        IsReusable = isReusable;
        MaxUses = maxUses;
    }

    public bool CanUse(int uses)
    {
        if (IsReusable)
        {
            if (MaxUses == null)
                return true;
            if (MaxUses > uses)
                return true;
            return false;
        }
        if (uses == 0)
            return true;
        return false;
    }
}