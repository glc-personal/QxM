using Qx.Domain.Labware.Models;

namespace Qx.Domain.Labware.States;

public sealed record SealState(SealStateKind Kind, DateTimeOffset? UnsealedAt)
{
    public static SealState Sealed() => new SealState(SealStateKind.Sealed, null);
    public static SealState Unsealed() => new SealState(SealStateKind.Unsealed, null);
    public static SealState Pierced(DateTimeOffset now) => new SealState(SealStateKind.Pierced, now);
}