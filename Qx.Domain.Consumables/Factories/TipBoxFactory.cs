using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Locations;
using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Consumables.Factories;

public sealed class TipBoxFactory
{
    private readonly DeckSlotConfiguration _config;
    
    public TipBoxFactory(DeckSlotConfiguration config)
    {
        _config = config;
    }

    public ITipBox Build(BatchNames batch, int numberOfColumns, int tipsPerColumn, ReusePolicy reusePolicy)
    {
        var tipBox = new TipBox(batch, numberOfColumns, tipsPerColumn, reusePolicy);
        return tipBox;
    }

    public IDictionary<BatchNames, ITipBox> Build(IList<BatchNames> batches, int numberOfColumns, int tipsPerColumn, ReusePolicy reusePolicy)
    {
        var tipBoxes = new Dictionary<BatchNames, ITipBox>();
        foreach (var batch in batches)
            tipBoxes[batch] = Build(batch, numberOfColumns, tipsPerColumn, reusePolicy);
        return tipBoxes;
    }
}