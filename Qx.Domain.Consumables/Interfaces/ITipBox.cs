using Qx.Domain.Consumables.Implementations;

namespace Qx.Domain.Consumables.Interfaces;

public interface ITipBox : IConsumable
{
    /// <summary>
    /// Add a tip column to the tip box
    /// </summary>
    /// <param name="tipColumn">Tip column to be added</param>
    void AddTips(TipColumn tipColumn);

    /// <summary>
    /// Remove a tip column from the tip box
    /// </summary>
    /// <param name="columnIndex">Column index of the tip column to be removed</param>
    /// <returns>The removed tip column</returns>
    TipColumn RemoveTips(int columnIndex);
}