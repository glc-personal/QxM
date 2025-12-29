namespace Qx.Domain.Labware.States;

public enum SealStateKind
{
    Sealed,             // sealed
    Unsealed,           // has never been sealed (cannot be pierced)
    Pierced             // seal has been broken/pierced
}