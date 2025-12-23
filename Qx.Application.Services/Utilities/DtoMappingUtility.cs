using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Utilities;

public static class DtoMappingUtility
{
    /// <summary>
    /// Map a <see cref="IConsumableType"/> to <see cref="ConsumableTypeDto"/>
    /// </summary>
    /// <param name="consumableType">consumable to map</param>
    /// <returns><see cref="ConsumableTypeDto"/></returns>
    public static ConsumableTypeDto MapToDto(IConsumableType consumableType)
    {
        var dto = new ConsumableTypeDto
        {
            Id = consumableType.UniqueIdentifier,
            Name = consumableType.Name,
            Version = consumableType.Version.ToString(),
            RowCount = consumableType.Geometry.RowCount,
            ColumnCount = consumableType.Geometry.ColumnCount,
            ColumnOffsetMm = consumableType.Geometry.ColumnOffsetMm,
            RowOffsetMm = consumableType.Geometry.RowOffsetMm,
            LengthMm = consumableType.Geometry.LengthMm,
            WidthMm = consumableType.Geometry.WidthMm,
            HeightMm = consumableType.Geometry.HeightMm,
            FirstColumnOffsetMm = consumableType.Geometry.FirstColumnOffsetMm,
            HeightOffDeckMm = consumableType.Geometry.HeightOffDeckMm,
            DefaultIsReusable = consumableType.DefaultIsReusable,
            DefaultMaxUses = consumableType.DefaultMaxUses
        };
        return dto;
    }
}