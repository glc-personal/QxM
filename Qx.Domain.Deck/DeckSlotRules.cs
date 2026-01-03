using Qx.Domain.Deck.Exceptions;

namespace Qx.Domain.Deck;

/// <summary>
/// Deck Slot Type Batch Rules
/// </summary>
public static class DeckSlotRules
{
    private static readonly HashSet<DeckSlotType> BatchlessTypes =
    [
        DeckSlotType.HeaterShaker,
        DeckSlotType.MagSeparator,
        DeckSlotType.Chiller,
        DeckSlotType.PreAmpThermocycler
    ];

    public static bool IsTypeBatchless(DeckSlotType type) 
        => BatchlessTypes.Contains(type);

    /// <summary>
    /// Create a deck slot name from the deck slot type and batch
    /// </summary>
    /// <param name="type"></param>
    /// <param name="batch"></param>
    /// <returns></returns>
    /// <exception cref="DeckSlotKeyException"></exception>
    public static string CreateDeckSlotName(DeckSlotType type, Batch? batch = null)
    {
        if (DeckSlotRules.IsTypeBatchless(type) && batch.HasValue)
            throw new DeckSlotKeyException(type, batch);
        if (!DeckSlotRules.IsTypeBatchless(type) && !batch.HasValue)
            throw new DeckSlotKeyException(type, batch);
        return !batch.HasValue ? type.ToString() : $"{type}-{batch}";
    }
}
