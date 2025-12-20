namespace Qx.Domain.Locations.Implementations;

/// <summary>
/// Column position
/// </summary>
/// <param name="ColumnIndex">Column index</param>
public sealed record ColumnPosition(int ColumnIndex) : Position;