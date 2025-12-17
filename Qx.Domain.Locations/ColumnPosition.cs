namespace Qx.Domain.Locations;

/// <summary>
/// Column position
/// </summary>
/// <param name="Index">Specifies which column</param>
public sealed record ColumnPosition(int Index) : Position;