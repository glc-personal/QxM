using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Tests.Domain.Labware;

[TestFixture]
public class LabwareGeometryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LabwareGeometry_Initialization_Tests()
    {
        // ensure initialization works with a grid and without a grid
        var labwareEnvelope = new LabwareEnvelope(new Millimeters(230m), new Millimeters(130m), new Millimeters(440m));
        LabwareGrid? labwareGrid = null;
        var labwareGeometry = new LabwareGeometry(labwareEnvelope, labwareGrid);
        Assert.That(labwareGeometry.Grid, Is.Null);
        labwareGrid = new LabwareGrid(8, 12, 
            new Millimeters(2m), new Millimeters(10m), new Millimeters(4m));
        labwareGeometry = new LabwareGeometry(labwareEnvelope, labwareGrid);
        Assert.That(labwareGeometry.Grid, Is.Not.Null);
    }
}