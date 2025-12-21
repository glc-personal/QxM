using Qx.Domain.Consumables.Interfaces;

namespace Qx.Domain.Consumables.Implementations;

public sealed class Inventory : IInventory
{
    private IList<IPlate> _plates = new List<IPlate>();
    private IList<ITipBox> _tipBoxes = new List<ITipBox>();

    public IReadOnlyList<IPlate> Plates => _plates.AsReadOnly();
    public IReadOnlyList<ITipBox> TipBoxes => _tipBoxes.AsReadOnly();
    
    public void AddPlate(IPlate plate)
    {
        if (PlateExistsByName(plate))
            throw new InvalidOperationException($"Inventory already has a {plate.Name}");
        _plates.Add(plate);
    }

    public void AddTipBox(ITipBox tipBox)
    {
        if (TipBoxExistsByName(tipBox))
            throw new ArgumentException($"Inventory already has a {tipBox.Name}");
        _tipBoxes.Add(tipBox);
    }

    public void RemovePlate(Guid plateId)
    {
        if (!PlateExistsById(plateId)) 
            throw new ArgumentException($"Inventory cannot remove plate (id: {plateId}) it does not exist");
        _plates.Remove(_plates.First(p => p.UniqueIdentifier == plateId));
    }

    public void RemoveTipBox(Guid tipBoxId)
    {
        if (!TipBoxExistsById(tipBoxId))
            throw new ArgumentException($"Inventory cannot remove tip box (id: {tipBoxId}) it does not exist");
        _tipBoxes.Remove(_tipBoxes.First(t => t.UniqueIdentifier == tipBoxId));
    }

    public IList<Guid> GetPlateIds()
    {
        var plateIds = new List<Guid>();
        plateIds.AddRange(Plates.Select(p => p.UniqueIdentifier));
        return plateIds;
    }

    public IList<Guid> GetTipBoxIds()
    {
        var tipBoxIds = new List<Guid>();
        tipBoxIds.AddRange(TipBoxes.Select(t => t.UniqueIdentifier));
        return tipBoxIds;
    }

    private bool PlateExistsByName(IPlate plate)
    {
        if (_plates.Select(p => p.Name).Contains(plate.Name))
            return true;
        return false;
    }

    private bool PlateExistsById(Guid plateId)
    {
        if (_plates.Select(p => p.UniqueIdentifier).Contains(plateId))
            return true;
        return false;
    }

    private bool TipBoxExistsByName(ITipBox tipBox)
    {
        if (_tipBoxes.Select(t => t.Name).Contains(tipBox.Name))
            return true;
        return false;
    }

    private bool TipBoxExistsById(Guid tipBoxId)
    {
        if (_tipBoxes.Select(t => t.UniqueIdentifier).Contains(tipBoxId))
            return true;
        return false;
    }
}