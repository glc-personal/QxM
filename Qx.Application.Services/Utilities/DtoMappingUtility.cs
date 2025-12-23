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
            RowCount = consumableType.RowCount,
            ColumnCount = consumableType.ColumnCount,
            Geometry = consumableType.Geometry,
            DefaultIsReusable = consumableType.DefaultIsReusable,
            DefaultMaxUses = consumableType.DefaultMaxUses
        };
        return dto;
    }
}