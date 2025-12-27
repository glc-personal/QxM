namespace Qx.Domain.Labware.Policies;

public sealed class ReusePolicy
{
    // ReSharper disable once MemberCanBePrivate.Global
    public bool IsReusable { get; }
    // ReSharper disable once MemberCanBePrivate.Global
    public int? MaxUses { get; } // null -> no limit
    
    private ReusePolicy(bool isReusable = true, int? maxUses = null)
    {
        if (!isReusable && maxUses != null)
            throw new ArgumentException("Max uses cannot be specified if not reusable.");
        if (maxUses is <= 0)
            throw new ArgumentException("Max uses must be greater than zero.");
        IsReusable = isReusable;
        MaxUses = maxUses;
    }
    
    /// <summary> Non-reusable reuse policy </summary>
    public static ReusePolicy NonReusable() => new ReusePolicy(false, null);
    /// <summary> Limited use reuse policy </summary>
    public static ReusePolicy Limited(int maxUses) => new ReusePolicy(true, maxUses);
    /// <summary> Unlimited use reuse policy </summary>
    public static ReusePolicy Unlimited() => new ReusePolicy(true, null);

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