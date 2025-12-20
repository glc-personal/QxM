using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Consumables.Utilities;

public static class ConsumableNamingUtility
{
    /// <summary>
    /// Create a consumable name
    /// </summary>
    /// <param name="deckSlotName">Deck slot name</param>
    /// <param name="batch">Batch name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(DeckSlotNames deckSlotName, BatchNames batch, int columnIndex)
    {
        if (batch == BatchNames.None)
            return CreateConsumableName(deckSlotName, columnIndex);
        return $"{deckSlotName}-{batch}-{CreateColumnName(columnIndex)}";
    }

    public static string CreateConsumableName(DeckSlotNames deckSlotName, BatchNames batch)
    {
        if (batch == BatchNames.None)
        {
            BatchNames[] validBatches = [BatchNames.A, BatchNames.B, BatchNames.C, BatchNames.D];
            throw new ArgumentException($"Invalid batch name ({batch}), must be from {validBatches.ToString()}");
        }
        return $"{deckSlotName}-{batch}";
    }

    /// <summary>
    /// Create a consumable name
    /// </summary>
    /// <param name="deckSlotName">Deck slot name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(DeckSlotNames deckSlotName, int columnIndex)
    {
        return $"{deckSlotName}-{CreateColumnName(columnIndex)}";
    }

    /// <summary>
    /// Create a consumable name
    /// </summary>
    /// <param name="readerSlotName">Reader slot name</param>
    /// <param name="batch">Batch name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(ReaderSlotNames readerSlotName, BatchNames batch, int columnIndex)
    {
        if (batch == BatchNames.None)
            return CreateConsumableName(readerSlotName, columnIndex);
        return $"{readerSlotName}-{batch}-{CreateColumnName(columnIndex)}";
    }

    /// <summary>
    /// Create a consumable name
    /// </summary>
    /// <param name="readerSlotName">Reader slot name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(ReaderSlotNames readerSlotName, int columnIndex)
    {
        return $"{readerSlotName}-{CreateColumnName(columnIndex)}";
    }

    /// <summary>
    /// Create column name for a consumable based on the column index (incremented by 1)
    /// </summary>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateColumnName(int columnIndex)
    {
        return $"Column{columnIndex+1}";
    }
}