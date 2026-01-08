namespace QxM.DeckManagement.Domain.Deck.Exceptions;

public class DeckDuplicateSlotException()
    : ArgumentException("Deck cannot have duplicate slots");