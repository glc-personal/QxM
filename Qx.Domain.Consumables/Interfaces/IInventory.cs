namespace Qx.Domain.Consumables.Interfaces;

public interface IInventory
{
    IReadOnlyList<IPlate> Plates { get; }
    IReadOnlyList<ITipBox> TipBoxes { get; }
    
    void AddPlate(IPlate plate);
    void AddTipBox(ITipBox tipBox);
    void RemovePlate(Guid plateId);
    void RemoveTipBox(Guid tipBoxId);
    IList<Guid> GetPlateIds();
    IList<Guid> GetTipBoxIds();
}