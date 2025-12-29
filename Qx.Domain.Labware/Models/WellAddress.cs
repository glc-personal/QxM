namespace Qx.Domain.Labware.Models;

public sealed record WellAddress(int RowIndex, int ColumnIndex)
{
    public override string ToString() 
        => $"[{RowIndex}, {ColumnIndex}]";
}