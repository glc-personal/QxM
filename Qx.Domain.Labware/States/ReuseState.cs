namespace Qx.Domain.Labware.States;

public sealed record ReuseState(int UseCount, DateTimeOffset? FirstUse, DateTimeOffset? LastUse)
{
    public static ReuseState New() => new ReuseState(0, null, null);

    public ReuseState Use(DateTimeOffset now) 
        => new ReuseState(UseCount: UseCount + 1, FirstUse: FirstUse ?? now, LastUse: now);
}