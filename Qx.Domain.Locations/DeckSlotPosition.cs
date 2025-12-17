namespace Qx.Domain.Locations;

/// <summary>
/// Deck slot position to specify where on the work-deck a position is
/// based on a column and row index.
/// </summary>
/// <param name="ColumnIndex">Column index of the position based on how far it is from home along the x-axis</param>
/// <param name="RowIndex">Row index of the position based on how far it is from home along the y-axis</param>
public sealed record DeckSlotPosition(int ColumnIndex, int RowIndex) : Position;