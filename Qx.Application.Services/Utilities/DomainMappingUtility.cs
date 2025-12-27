using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;
using Version = Qx.Core.Version;

namespace Qx.Application.Services.Utilities;

public static class DomainMappingUtility
{
    public static IConsumableType MapToDomain(ConsumableTypeDto dto)
    {
        var typeEnum = dto.Type;
        var version = Version.Parse(dto.Version);
        var geometry = new ConsumableGeometry
        {
            ColumnCount = dto.ColumnCount,
            RowCount = dto.RowCount,
            ColumnOffsetMm = dto.ColumnOffsetMm,
            RowOffsetMm = dto.RowOffsetMm,
            LengthMm = dto.LengthMm,
            WidthMm = dto.WidthMm,
            HeightMm = dto.HeightMm,
            FirstColumnOffsetMm = dto.FirstColumnOffsetMm,
            HeightOffDeckMm = dto.HeightOffDeckMm,
        };
        
        return new ConsumableType(typeEnum, geometry, version, dto.DefaultIsReusable, dto.DefaultMaxUses);
    }

    public static Location MapToDomain(LocationDto dto)
    {
        var id = dto.Id;
        var name = dto.Name;
        var position = new CoordinatePosition(dto.XUs, dto.YUs, dto.ZUs, CoordinatePositionUnits.Microsteps);
        var frame = (CoordinateFrame)Enum.Parse(typeof(CoordinateFrame), dto.Frame);
        var location = new Location(id, name, position, frame, dto.IsCustom);
        return location;
    }

    public static IConsumable MapToDomain(ConsumableDto dto)
    {
        var name = dto.Name;
        var slot = ConsumableNamingUtility.GetDeckSlotNameFromConsumableName(name);
        var batch = ConsumableNamingUtility.GetBatchNameFromConsumableName(name);
        var type = (ConsumableType)MapToDomain(dto.Type);
        var location = MapToDomain(dto.Location);
        var reusePolicy = new ReusePolicy(dto.IsReusable, dto.MaxUses);
        var sealPolicy = new SealPolicy(dto.IsSealed);
        return new Consumable(slot, batch, type, location, reusePolicy, sealPolicy);
    }
}