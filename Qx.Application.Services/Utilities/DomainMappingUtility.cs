using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Version = Qx.Core.Version;

namespace Qx.Application.Services.Utilities;

public static class DomainMappingUtility
{
    public static IConsumableType MapToDomain(ConsumableTypeDto dto)
    {
        var typeEnum = (ConsumableTypes)Enum.Parse(typeof(ConsumableTypes), dto.Name);
        var version = Version.Parse(dto.Version);
        var geometry = new ConsumableGeometry(dto.ColumnCount, dto.RowCount,
            dto.ColumnOffsetMm, dto.RowOffsetMm,
            dto.LengthMm, dto.WidthMm, dto.HeightMm,
            dto.FirstColumnOffsetMm, dto.HeightOffDeckMm);
        
        return new ConsumableType(typeEnum, geometry, version, dto.DefaultIsReusable, dto.DefaultMaxUses);
    }
}