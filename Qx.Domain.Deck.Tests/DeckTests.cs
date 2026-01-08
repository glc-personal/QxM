using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;
using Version = Qx.Core.Version;

namespace Qx.Domain.Deck.Tests;

[TestFixture]
public class DeckTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DeckDefinitionIncompleteSlotListTest()
    {
        var slotDefs = new List<DeckSlotDefinition>
        {
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.A),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.B),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.C),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.D),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.A),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.B),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.ReagentCartridge, Batch.A),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Chiller, null),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.HeaterShaker, null),
                new DeckSlotPose(new Mm(0.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
        };
        
        var geometry = new DeckGeometry(new Mm(10.0m), new Mm(10.0m));
        
        Assert.Catch<DeckMissingSlotException>(() => { DeckDefinition.Create(geometry, slotDefs); });
    }

    [Test]
    public void DeckDefinitionGeometryRangeTest()
    {
        var slotDefs = SetupDeckSlotDefinitions();
        var geometry = new DeckGeometry(new Mm(10.0m), new Mm(110.0m));

        Assert.Catch<DeckSlotPoseException>(() =>
        {
            DeckDefinition.Create(geometry, slotDefs);
        });
    }

    [TestCase(DeckSlotType.CartridgeTray, Batch.A)]
    [TestCase(DeckSlotType.HeaterShaker, null)]
    public void DeckLabwareTest(DeckSlotType slotType, Batch? batch = null)
    {
        var slotDefs = SetupDeckSlotDefinitions();
        var geometry = new DeckGeometry(new Mm(1000.0m), new Mm(1000.0m));
        var deckDefinition = DeckDefinition.Create(geometry, slotDefs);
        var deck = Deck.Create(deckDefinition);
        var deckSlotKey = new DeckSlotKey(slotType, batch);
        
        // Make sure the slot is not occupied and has no labware
        var deckSlotState = deck.GetDeckSlotState(deckSlotKey);
        Assert.Multiple(() =>
        {
            Assert.That(deckSlotState.Occupancy, Is.EqualTo(Occupancy.Unoccupied));
            Assert.That(deckSlotState.Labware, Is.Null);
        });

        // add labware
        var labwareName = slotType.ToString();
        var labwareRef = new LabwareDefinitionReference(LabwareId.New(), new Version(1, 0, 0, 0), labwareName);
        deck.AddLabware(labwareRef, deckSlotKey);
        
        // make sure the slot is occupied with the correct labware
        deckSlotState = deck.GetDeckSlotState(deckSlotKey);
        Assert.Multiple(() =>
        {
            Assert.That(deckSlotState.Occupancy, Is.EqualTo(Occupancy.Occupied));
            Assert.That(deckSlotState.Labware, Is.EqualTo(labwareRef));
        });
        
        // remove the labware
        deck.RemoveLabware(deckSlotKey);
        deckSlotState = deck.GetDeckSlotState(deckSlotKey);
        Assert.Multiple(() =>
        {
            Assert.That(deckSlotState.Occupancy, Is.EqualTo(Occupancy.Unoccupied));
            Assert.That(deckSlotState.Labware, Is.Null);
        });
    }

    private List<DeckSlotDefinition> SetupDeckSlotDefinitions()
    {
        return
        [
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.QuantStrip, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.TipRack, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.ReagentCartridge, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.ReagentCartridge, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.ReagentCartridge, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.ReagentCartridge, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.SampleRack, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.SampleRack, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.SampleRack, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.SampleRack, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.RnaHeater, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.RnaHeater, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.RnaHeater, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.RnaHeater, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.AssayPrepStrip, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.AssayPrepStrip, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.AssayPrepStrip, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.AssayPrepStrip, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Tray, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Tray, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Tray, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Tray, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.DropletGenerator, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.DropletGenerator, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.DropletGenerator, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.DropletGenerator, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.LidTray, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.LidTray, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.LidTray, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.LidTray, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.CartridgeTray, Batch.A),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.CartridgeTray, Batch.B),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.CartridgeTray, Batch.C),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.CartridgeTray, Batch.D),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.Chiller, null),
                new DeckSlotPose(new Mm(150.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.PreAmpThermocycler, null),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.HeaterShaker, null),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m))),
            new DeckSlotDefinition(new DeckSlotKey(DeckSlotType.MagSeparator, null),
                new DeckSlotPose(new Mm(10.0m), new Mm(0.0m), new AngleDegrees(0.0m)))
        ];
    }
}