using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;

namespace Qx.Application.Services.Utilities;

public static class DomainMappingUtility
{
    public static IConsumableType MapToDomain(ConsumableTypeDto dto)
    {
        var typeEnum = (ConsumableTypes)Enum.Parse(typeof(ConsumableTypes), dto.Name);
        var consumableType = new ConsumableType(typeEnum, dto.RowCount, dto.ColumnCount, dto.Geometry);
        return consumableType;
    }
}