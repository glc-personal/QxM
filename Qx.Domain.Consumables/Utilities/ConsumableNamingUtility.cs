using System.Diagnostics.CodeAnalysis;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Consumables.Utilities;

/// <summary>
/// Utility for dealing with consumable names which have the structure
/// {deckSlotName}-{batchName}-Column{columnIndex+1} if the consumable has a batch associated with it
/// {deckSlotName}-Column{columnIndex+1} if the consumable does not have a batch associated with it
/// </summary>
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

    public static DeckSlotNames GetDeckSlotNameFromConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        if (nameArray.Length < 2)
            throw new ConsumableNameFormatException(consumableName);
        var deckSlotName = nameArray[new Range(0, nameArray.Length - 2)];
        if (deckSlotName.Length > 1)
            throw new ConsumableNameFormatException(consumableName);
        return (DeckSlotNames)Enum.Parse(typeof(DeckSlotNames), deckSlotName[0]);
    }

    public static BatchNames GetBatchNameFromConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        if (nameArray.Length == 2) return BatchNames.None;
        if (nameArray.Length < 2) throw new ConsumableNameFormatException(consumableName);
        return (BatchNames)Enum.Parse(typeof(BatchNames), nameArray[^2]);
    }

    public static string GetColumnNameFromConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        if (nameArray.Length < 2) throw new ConsumableNameFormatException(consumableName);
        var columnName = nameArray[^1];
        if (IsColumnName(columnName)) return columnName;
        throw new ConsumableNameFormatException(consumableName);
    }

    public static int GetColumnIndexFromConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        if (nameArray.Length < 2) throw new ConsumableNameFormatException(consumableName);
        var columnIndex = SplitColumnIndexFromColumnName(nameArray[^1], consumableName);
        return columnIndex;
    }

    private static bool HasBatchInConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        // check the classic case ({deckSlotName}-{batchName}-Column{columnIndex} and {deckSlotName}-Column{columnIndex})
        if (nameArray.Length >= 3)
            return IsBatchName(nameArray[^2]);
        if (nameArray.Length == 2)
            return false;
        throw new ConsumableNameFormatException(consumableName);
    }

    private static bool IsDeckSlotName(string value)
    {
        var validNames = Enum.GetValues(typeof(DeckSlotNames)).Cast<string>();
        return validNames.Contains(value);
    }

    private static bool IsBatchName(string value)
    {
        var validBatchNames = Enum.GetValues(typeof(BatchNames)).Cast<string>().ToList();
        validBatchNames.Remove(BatchNames.None.ToString());
        return validBatchNames.Contains(value);
    }

    private static bool IsColumnName(string value)
    {
        return value.Contains("Column") && Int32.TryParse(value.Replace("Column", string.Empty), out var columnIndex);
    }

    private static int SplitColumnIndexFromColumnName(string columnName, string consumableName)
    {
        if (columnName.Contains("Column") && Int32.TryParse(columnName.Replace("Column", string.Empty), out var columnIndex)) return columnIndex - 1;
        throw new FormatException($"Invalid column name ({columnName}) in {consumableName}");
    }
}