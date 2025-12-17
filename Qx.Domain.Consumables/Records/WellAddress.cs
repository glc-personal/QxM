using Qx.Domain.Consumables.Exceptions;

namespace Qx.Domain.Consumables.Records;

public sealed record WellAddress
{
    public int Row { get; init; }
    public int Column { get; init; }

    public WellAddress(int row, int column)
    {
        Row = row;
        Column = column;

        if (!ValidRow(row) || !ValidColumn(column))
            throw new InvalidWellAddressException(this);
    }

    private static bool ValidRow(int row)
    {
        return row >= 0;
    }

    private static bool ValidColumn(int column)
    {
        return column >= 0;
    }
}