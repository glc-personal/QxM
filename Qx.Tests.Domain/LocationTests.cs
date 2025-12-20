using Qx.Domain.Locations;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Exceptions;
using Qx.Domain.Locations.Implementations;
using Qx.Domain.Locations.Utilities;
using Qx.Tests.Domain.Utilities;

namespace Qx.Tests.Domain;

[TestFixture]
public class LocationTests
{
    private DeckSlotConfiguration _deckSlotConfiguration;
    private DeckSlotUtility _deckSlotUtility;
    
    [SetUp]
    public void Setup()
    {
        SetupDeckSlotConfiguration();
    }

    // TODO: Split this test up, it covers too many things
    [Test]
    public void DeckSlotTests()
    {
        
        // ensure the deck slot util can get the coordinate position of the deck slot
        var expectedCoordPos = new CoordinatePosition(150000, 300000, 20000, CoordinatePositionUnits.Microsteps);
        var deckSlotPos = new DeckSlotPosition(DeckSlotNames.TipBox, BatchNames.A);
        var actualCoordPos = _deckSlotUtility.GetCoordinatePosition(deckSlotPos);
        Assert.AreEqual(actualCoordPos, expectedCoordPos);
        
        // ensure invalid configurations are caught for a missing key
        var badKeyPosition = new DeckSlotPosition(DeckSlotNames.TipBox, BatchNames.B);
        Assert.Catch<ConfigurationKeyNotFoundException>(() => _deckSlotUtility.GetCoordinatePosition(badKeyPosition));
    }

    [Test]
    public void LocationUseTests()
    {
        // Create a location not in the config but based off it (include the column)
        var deckSlotName = DeckSlotNames.TipBox;
        var batch = BatchNames.A;
        var columnIndex = 1; 
        var locationName = deckSlotName + "-" + batch + $"-Column{columnIndex}";
        var deckSlotPos = new DeckSlotPosition(deckSlotName, batch);
        // look up the configured pos of the deck slot pos
        var pos = _deckSlotUtility.GetCoordinatePosition(deckSlotPos);
        var location = new Location(locationName, pos);
        Assert.Pass();
    }

    private void SetupDeckSlotConfiguration()
    {
        // create a sample deck slot json object to configure the deck slot utility
        var jsonObject = TestHelperUtility.CreateDeckSlotConfigObject(
            DeckSlotNames.TipBox, BatchNames.A, 
            new CoordinatePosition(150000, 300000, 20000, CoordinatePositionUnits.Microsteps)
            );
        
        // configure the deck slot utility
        var _deckSlotConfiguration = new DeckSlotConfiguration();
        _deckSlotConfiguration.Configure(jsonObject);
        _deckSlotUtility = new DeckSlotUtility(_deckSlotConfiguration);
    }
}