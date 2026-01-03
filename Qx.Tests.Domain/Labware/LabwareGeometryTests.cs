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
        var labwareEnvelope = new LabwareEnvelope(new Mm(230m), new Mm(130m), new Mm(440m));
        LabwareGrid? labwareGrid = null;
        var labwareGeometry = new LabwareGeometry(labwareEnvelope, labwareGrid);
        Assert.That(labwareGeometry.Grid, Is.Null);
        labwareGrid = new LabwareGrid(8, 12, 
            new Mm(2m), new Mm(10m), new Mm(4m));
        labwareGeometry = new LabwareGeometry(labwareEnvelope, labwareGrid);
        Assert.That(labwareGeometry.Grid, Is.Not.Null);
    }
}