namespace Qx.Domain.Labware.States;

/// <summary>
/// Represents the state for a column of tips
/// </summary>
/// <param name="ColumnIndex">Column index</param>
/// <param name="Kind">State the tips are in</param>
public sealed record TipState(int ColumnIndex, TipStateKind Kind)
{
    public static TipState InRack(int columnIndex) => new TipState(columnIndex, TipStateKind.InRack);
    public static TipState NotInRack(int columnIndex) => new TipState(columnIndex, TipStateKind.NotInRack);
    public static TipState EngagedToPipetteHead(int columnIndex) => 
        new TipState(columnIndex, TipStateKind.EngagedToPipetteHead);
}