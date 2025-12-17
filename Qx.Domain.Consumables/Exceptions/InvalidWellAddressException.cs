using Qx.Domain.Consumables.Records;

namespace Qx.Domain.Consumables.Exceptions;

public class InvalidWellAddressException(WellAddress wellAddress)
    : ArgumentException(paramName: nameof(wellAddress), message: BuildMessage(wellAddress))
{
    private static string BuildMessage(WellAddress? wellAddress)
    {
        var message = "Invalid well address:";
        if (wellAddress == null)
            return $"{message} (cannot be null)";

        var badRow = wellAddress.Row < 0;
        var badColumn = wellAddress.Column < 0;
        return (badRow, badColumn) switch
        {
            (true, true) => $"{message} row ({wellAddress.Row}) and column ({wellAddress.Column}) must be non-negative",
            (true, false) => $"{message} row ({wellAddress.Row}) must be non-negative",
            (false, true) => $"{message} column ({wellAddress.Column}) must be non-negative",
            (false, false) => $"Well address '{wellAddress}' is valid", // should never be caught
        };
    }
}