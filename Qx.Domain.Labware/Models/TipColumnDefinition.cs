namespace Qx.Domain.Labware.Models;

// Domain invariant: tip columns have all the same tips (this is why we have a single tip definition and not one per tip)
public sealed record TipColumnDefinition(int ColumnIndex, TipDefinition TipDefinition);