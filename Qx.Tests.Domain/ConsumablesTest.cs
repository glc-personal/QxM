using System.ComponentModel;
using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Exceptions;
using Qx.Domain.Consumables.Implementations;
using Qx.Domain.Consumables.Interfaces;
using Qx.Domain.Consumables.Records;
using Qx.Domain.Consumables.Utilities;
using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Exceptions;
using Qx.Domain.Liquids.Records;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;

namespace Qx.Tests.Domain;

[TestFixture]
public class ConsumablesTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ConsumableNamingUtility_Test()
    {
        // ensure attempting to create a consumable name with a deck slot and batch fails if batch is set to none
        Assert.Catch<ArgumentException>(() =>
        {
            var name = ConsumableNamingUtility.CreateConsumableName(SlotName.TipBox, BatchNames.None);
        });
    }
}