using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Consumables.Exceptions;

public class ConsumableNameFormatException(string consumableName) 
    : FormatException($"Consumable name ({consumableName}) format is invalid. Expect: {nameof(DeckSlotNames)}-{nameof(BatchNames)}-Column# or {nameof(DeckSlotNames)}-Column#")
{
}