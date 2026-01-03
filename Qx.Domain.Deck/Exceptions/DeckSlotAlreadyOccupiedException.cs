namespace Qx.Domain.Deck.Exceptions;

public class DeckSlotAlreadyOccupiedException(DeckSlotKey key) 
    : InvalidOperationException($"Deck slot ({key.Label}) is already occupied");