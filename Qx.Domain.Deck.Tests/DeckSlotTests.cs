using Qx.Core.Measurement;
using Qx.Domain.Deck.Exceptions;
using Qx.Domain.Labware.LabwareDefinitions;
using Version = Qx.Core.Version;

namespace Qx.Domain.Deck.Tests;

public class DeckSlotTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(DeckSlotType.Tray, Batch.A)]
    [TestCase(DeckSlotType.Tray, Batch.B)]
    [TestCase(DeckSlotType.TipRack, Batch.C)]
    [TestCase(DeckSlotType.ReagentCartridge, Batch.D)]
    [TestCase(DeckSlotType.HeaterShaker)]
    [TestCase(DeckSlotType.Chiller)]
    [TestCase(DeckSlotType.PreAmpThermocycler)]
    [TestCase(DeckSlotType.HeaterShaker, Batch.C)]
    [TestCase(DeckSlotType.MagSeparator, Batch.D)]
    [TestCase(DeckSlotType.PreAmpThermocycler, Batch.A)]
    [TestCase(DeckSlotType.QuantStrip)]
    [TestCase(DeckSlotType.ReagentCartridge)]
    public void DeckSlotKeyTest(DeckSlotType type, Batch? batch = null)
    {
        var label = !batch.HasValue ? type.ToString() : $"{type}-{batch}";
        if ((DeckSlotRules.IsTypeBatchless(type) && batch.HasValue) || (!DeckSlotRules.IsTypeBatchless(type) && !batch.HasValue))
        {
            Assert.Catch<DeckSlotKeyException>(() => { var actualName = new DeckSlotKey(type, batch); });
            return;
        }
        var actual = new DeckSlotKey(type, batch);
        Assert.That(label, Is.EqualTo(actual.Label));
    }

    /// <summary>
    /// Test domain invariant: single occupancy
    /// </summary>
    [Test]
    public void DeckSlotSingleOccupancyTest()
    {
        var deckSlot = CreateDeckSlot(DeckSlotType.TipRack, Batch.A);
        var name = deckSlot.Key.Type.ToString();
        var labwareRef = new LabwareDefinitionReference(LabwareId.New(), new Version(1, 0, 0, 0), name);
        
        // add valid labware
        deckSlot.AddLabware(labwareRef);
        
        // try adding more valid labware
        Assert.Catch<DeckSlotAlreadyOccupiedException>(() => deckSlot.AddLabware(labwareRef));
    }

    /// <summary>
    /// Test domain invariant: valid labware in a deck slot
    /// </summary>
    [TestCase(DeckSlotType.TipRack, DeckSlotType.ReagentCartridge, Batch.A, Batch.B)]
    [TestCase(DeckSlotType.TipRack, DeckSlotType.Chiller, Batch.A, null)]
    public void DeckSlotValidOccupancyTest(DeckSlotType deckSlotType, DeckSlotType labwareType, 
        Batch? deckSlotBatch = null, Batch? labwareBatch = null)
    {
        var deckSlot = CreateDeckSlot(DeckSlotType.TipRack, Batch.A);
        var name = DeckSlotRules.CreateDeckSlotName(labwareType, labwareBatch);
        var labwareRef = new LabwareDefinitionReference(LabwareId.New(), new Version(1, 0, 0, 0), name);
        
        // add labware that doesn't go in this deck slot
        Assert.Catch<DeckSlotTypeAndLabwareException>(() => deckSlot.AddLabware(labwareRef));
    }

    private DeckSlot CreateDeckSlot(DeckSlotType type, Batch? batch = null)
    {
        var deckSlotX = new Mm(100.0m);
        var deckSlotY = new Mm(100.0m);
        var deckSlotTheta = AngleDegrees.Zero;
        var deckSlotDefinition = new DeckSlotDefinition
        {
            Key = new DeckSlotKey(type, batch),
            Pose = new DeckSlotPose(deckSlotX, deckSlotY, deckSlotTheta)
        };
        var deckSlot = DeckSlot.Create(deckSlotDefinition);
        return deckSlot;
    }
}