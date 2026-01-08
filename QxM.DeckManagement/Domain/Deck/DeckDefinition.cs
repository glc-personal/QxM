using QxM.DeckManagement.Domain.Deck.Exceptions;

namespace QxM.DeckManagement.Domain.Deck;

public sealed class DeckDefinition
{
    private DeckDefinition(DeckGeometry geometry, IReadOnlyList<DeckSlotDefinition> slotDefinitions)
    {
        Geometry = geometry;
        SlotDefinitions = slotDefinitions;
    }
    
    public DeckGeometry Geometry { get; }
    public IReadOnlyList<DeckSlotDefinition> SlotDefinitions { get; }

    public static DeckDefinition Create(DeckGeometry geometry, List<DeckSlotDefinition> slotDefinitions)
    {
        EnforceExhaustiveDeckSlotList(slotDefinitions);
        EnforceSlotPoseWithinDeckGeometry(geometry, slotDefinitions);
        EnforceNoDuplicates(slotDefinitions);
        return new DeckDefinition(geometry, slotDefinitions);
    }

    private static void EnforceExhaustiveDeckSlotList(List<DeckSlotDefinition> slotDefinitions)
    {
        // get all deck slot keys and check if they have been found (default: false) or not in our input
        var exhaustiveList = new Dictionary<DeckSlotKey, bool>();
        foreach (var type in Enum.GetValues<DeckSlotType>())
        {
            if (DeckSlotRules.IsTypeBatchless(type))
                exhaustiveList.Add(new DeckSlotKey(type, null), false);
            else
            {
                foreach (var batch in Enum.GetValues<Batch>())
                    exhaustiveList.Add(new DeckSlotKey(type, batch), false);
            }
        }
        
        // check if found
        foreach (var slotDefinition in slotDefinitions)
        {
            if (exhaustiveList.ContainsKey(slotDefinition.Key))
                exhaustiveList[slotDefinition.Key] = true;
        }
        
        // check if something wasn't found
        var missingKeys = exhaustiveList.Where(kvp => kvp.Value == false).Select(kvp => kvp.Key).ToList();
        if (missingKeys.Count != 0)
            throw new DeckMissingSlotException(missingKeys);
    }

    private static void EnforceSlotPoseWithinDeckGeometry(DeckGeometry geometry,
        IEnumerable<DeckSlotDefinition> slotDefinitions)
    {
        // TODO: Make this not fail one at a time, fail all together so they can be all fixed together, not fix one, fail one, fix one, etc
        foreach (var slotDefinition in slotDefinitions)
        {
            var slotPose = slotDefinition.Pose;
            if (slotPose.X.Value > geometry.DepthX.Value || slotPose.Y.Value > geometry.LengthY.Value)
                throw new DeckSlotPoseException(slotPose, geometry);
        }
    }

    private static void EnforceNoDuplicates(List<DeckSlotDefinition> slotDefinitions)
    {
        var keys = slotDefinitions.Select(slotDefinition => slotDefinition.Key).ToList();
        var expectedKeys = GetAllExpectedDeckSlotKeys();
        if (keys.Count > expectedKeys.Count)
            throw new DeckDuplicateSlotException();
    }

    private static List<DeckSlotKey> GetAllExpectedDeckSlotKeys()
    {
        var allKeys = new List<DeckSlotKey>();
        foreach (var type in Enum.GetValues<DeckSlotType>())
        {
            if (DeckSlotRules.IsTypeBatchless(type))
                allKeys.Add(new DeckSlotKey(type, null));
            else
                allKeys.AddRange(Enum.GetValues<Batch>().Select(batch => new DeckSlotKey(type, batch)));
        }

        return allKeys;
    }
}