namespace Qx.Domain.Labware.States;

public enum TipStateKind
{
    InRack,                 // tips are in the tip rack
    NotInRack,              // tips are not in the tip rack and not engaged to the pipette head
    EngagedToPipetteHead,   // tips are engaged to the pipette head
}