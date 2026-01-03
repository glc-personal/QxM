namespace Qx.Domain.Deck.Exceptions;

public class DeckSlotKeyException(DeckSlotType type, Batch? batch) 
    : ArgumentException(BuildMessage(type, batch))
{
    private static string BuildMessage(DeckSlotType type, Batch? batch)
    {
        var msgPart1 = $"{nameof(DeckSlotType)} ({type}) ";
        var msgPart2 = string.Empty;
        var msgPart3 = string.Empty;
        if (DeckSlotRules.IsTypeBatchless(type) && batch.HasValue)
        {
            msgPart2 = $"does not have an associated batch, ";
            msgPart3 = $"expected a {nameof(DeckSlotKey)} ({type}) but got {type}-{batch}";
        }

        if (!DeckSlotRules.IsTypeBatchless(type) && !batch.HasValue)
        {
            msgPart2 = $" has an associated batch, ";
            msgPart3 = $"expected a {nameof(DeckSlotKey)} ({type}-<batch>) but got {type}";
        }
        
        return msgPart1 + msgPart2 + msgPart3;
    }
}