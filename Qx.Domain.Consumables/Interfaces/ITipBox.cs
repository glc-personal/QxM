using Qx.Domain.Consumables.Implementations;

namespace Qx.Domain.Consumables.Interfaces;

public interface ITipBox : IConsumable
{
    /// <summary>
    /// Number of columns in the tip box
    /// </summary>
    int NumberOfColumns { get; }
    
    /// <summary>
    /// Number of rows in the tip box (number of tips per column)
    /// </summary>
    int NumberOfRows { get; }
    
    /// <summary>
    /// Add new tips to a column
    /// </summary>
    /// <param name="tip">Tip to be replicated and added to the column</param>
    /// <param name="columnIndex">Column index</param>
    void AddNewTips(ITip tip, int columnIndex);

    /// <summary>
    /// Add new tips to a set of columns
    /// </summary>
    /// <param name="tip"></param>
    /// <param name="columnIndexes"></param>
    /// <param name="failOnOne">Fail immediately if one index already contains tips</param>
    void AddNewTips(ITip tip, IList<int> columnIndexes, bool failOnOne = true);
    
    /// <summary>
    /// Add tips to the tip box
    /// </summary>
    /// <param name="tips">Tips to be added</param>
    /// <param name="columnIndex">Column where the tips will be added</param>
    void AddTips(IList<ITip> tips, int columnIndex);

    /// <summary>
    /// Remove a tip column from the tip box
    /// </summary>
    /// <param name="columnIndex">Column index of the tip column to be removed</param>
    /// <returns>The removed tips</returns>
    IList<ITip> RemoveTips(int columnIndex);
}