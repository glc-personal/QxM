namespace Qx.Domain.Labware.States;

public enum ReuseStateKind
{
    Reusable,       // reusable
    NotReusable,    // not reusable (can only be used once)
    OutOfUses,      // out of uses (only if it was reusable)
}