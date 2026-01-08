using System.Text;

namespace QxM.DeckManagement.Domain.Deck.Exceptions;

public class DeckMissingSlotException(List<DeckSlotKey> missingKeys) : KeyNotFoundException(BuildMessage(missingKeys))
{
    private static string BuildMessage(List<DeckSlotKey> missingKeys)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Missing {missingKeys.Count} {nameof(DeckSlotKey)}s:");
        foreach (var missingKey in missingKeys)
        {
            stringBuilder.AppendLine($" - {missingKey.Label}");
        }
        return stringBuilder.ToString();
    }
}