using Qx.Domain.Deck.Exceptions;

namespace Qx.Domain.Deck;

/// <summary>
/// Deck slot key
/// </summary>
public readonly record struct DeckSlotKey
{
    public DeckSlotKey(DeckSlotType type, Batch? batch)
    {
        Type = type;
        Batch = batch;
        Label = DeckSlotRules.CreateDeckSlotName(type, batch);
    }
    public DeckSlotType Type { get; }
    public Batch? Batch { get; }
    public string Label { get; }
}