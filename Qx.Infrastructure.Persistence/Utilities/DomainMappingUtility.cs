using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Liquids.Enums;
using Qx.Infrastructure.Persistence.Entities;
using Version = Qx.Core.Version;

namespace Qx.Infrastructure.Persistence.Utilities;

public static class DomainMappingUtility
{
    public static IInventory MapToDomain(InventoryEntity entity)
    {
        var inventory = new Inventory();
        
        //foreach (var consumable in entity.Consumables)
        return inventory;
    }

    public static IConsumable MapToDomain(ConsumableEntity entity)
    {
        var name = entity.Name;
        var deckSlotName = ConsumableNamingUtility.GetDeckSlotNameFromConsumableName(name);
        var batchName = ConsumableNamingUtility.GetBatchNameFromConsumableName(name);

        return new Consumable(deckSlotName, batchName, ConsumableTypes.TipBox, 
            0,0,
            new ReusePolicy(true), 
            new SealPolicy(false));
    }

    public static IConsumableType MapToDomain(ConsumableTypeEntity entity)
    {
        var name = (ConsumableTypes)Enum.Parse(typeof(ConsumableTypes), entity.Name);
        var version = Version.Parse(entity.Version);
        var geometry = new ConsumableGeometry(entity.ColumnCount, entity.RowCount,
            entity.ColumnOffsetMm, entity.RowOffsetMm, 
            entity.LengthMm, entity.WidthMm, entity.HeightMm,
            entity.FirstColumnOffsetMm, entity.HeightOffDeckMm);
        
        return new ConsumableType(name, geometry, version, entity.DefaultIsReusable, entity.DefaultMaxUses);
    }

    private static ConsumableColumn MapToDomain(ConsumableColumnEntity entity)
    {
        var name = entity.Name;
        var columnIndex = ConsumableNamingUtility.GetColumnIndexFromConsumableName(name);
        var isReusable = entity.IsReusable;
        var maxUses = entity.MaxUses;
        var isSealed = entity.IsSealed;
        var reusePolicy = isReusable ? new ReusePolicy(isReusable, maxUses) : new ReusePolicy(isReusable);
        var capacitiesAsDoubles = entity.VolumeCapacityUl;
        var capacities = VolumeContainerUtility.ConvertDoublesToCapacities(capacitiesAsDoubles, VolumeUnits.Ul);

        return new ConsumableColumn(capacities, reusePolicy, new SealPolicy(isSealed));
    }
}