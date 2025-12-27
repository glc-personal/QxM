using System.Drawing;
using Qx.Application.Services.Dtos;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Locations.Exceptions;
using Qx.Domain.Locations.Implementations;

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
            Type = consumableType.Type,
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

    /// <summary>
    /// Map domain location to a location data type object
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    /// <exception cref="LocationPositionTypeException"></exception>
    public static LocationDto MapToDto(Location location)
    {
        if (location.Position.GetType() != typeof(CoordinatePosition))
            throw new LocationPositionTypeException(location.Position.GetType());
        var position = (CoordinatePosition)location.Position;
        var dto = new LocationDto
        {
            Id = location.UniqueIdentifier,
            Name = location.Name,
            XUs = position.X,
            YUs = position.Y,
            ZUs = position.Z,
            Frame = location.Frame.ToString(),
        };
        
        return dto;
    }
}