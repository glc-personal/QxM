using Qx.Domain.Labware.Policies;

namespace Qx.Domain.Labware.States;

public sealed record ReuseState(ReuseStateKind Kind, int UseCount, DateTimeOffset? FirstUse, DateTimeOffset? LastUse)
{
    public static ReuseState New(ReuseStateKind kind) => new ReuseState(kind, 0, null, null);

    public ReuseState Use(ReusePolicy reusePolicy, DateTimeOffset now)
    {
        if (Kind != ReuseStateKind.Reusable)
        {
            if (UseCount >= 1)
                throw new InvalidOperationException($"Cannot use again, current reuse state is {Kind}");
            return new ReuseState(Kind, UseCount + 1, now, now);
        }
        return new ReuseState((UseCount + 1 >= reusePolicy.MaxUses) ? ReuseStateKind.OutOfUses : ReuseStateKind.Reusable,
            UseCount + 1, FirstUse ?? now, now);
    }
}