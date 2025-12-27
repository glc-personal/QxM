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
    /// <param name="slotName">Deck slot name</param>
    /// <param name="batch">Batch name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(SlotName slotName, BatchNames batch, int? columnIndex)
    {
        if (columnIndex == null && batch == BatchNames.None)
            return slotName.ToString();
        if (columnIndex == null)
            return CreateConsumableName(slotName, batch);
        if (batch == BatchNames.None)
            return CreateConsumableName(slotName, columnIndex.Value);
        return $"{slotName}-{batch}-{CreateColumnName(columnIndex.Value)}";
    }

    public static string CreateConsumableName(SlotName slotName, BatchNames batch)
    {
        if (batch == BatchNames.None)
        {
            BatchNames[] validBatches = [BatchNames.A, BatchNames.B, BatchNames.C, BatchNames.D];
            throw new ArgumentException($"Invalid batch name ({batch}), must be from {validBatches.ToString()}");
        }
        return $"{slotName}-{batch}";
    }

    /// <summary>
    /// Create a consumable name
    /// </summary>
    /// <param name="slotName">Deck slot name</param>
    /// <param name="columnIndex">Column index</param>
    /// <returns></returns>
    public static string CreateConsumableName(SlotName slotName, int columnIndex)
    {
        return $"{slotName}-{CreateColumnName(columnIndex)}";
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

    public static SlotName GetDeckSlotNameFromConsumableName(string consumableName)
    {
        var nameArray = consumableName.Split('-');
        if (nameArray.Length < 2)
            throw new ConsumableNameFormatException(consumableName);
        var deckSlotName = nameArray[new Range(0, nameArray.Length - 2)];
        if (deckSlotName.Length > 1)
            throw new ConsumableNameFormatException(consumableName);
        return (SlotName)Enum.Parse(typeof(SlotName), deckSlotName[0]);
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
        var validNames = Enum.GetValues(typeof(SlotName)).Cast<string>();
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