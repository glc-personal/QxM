using Qx.Core.Measurement;
using Qx.Domain.Labware.Exceptions;
using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Tests.Domain.Labware;

[TestFixture]
public class LabwareEnvelopeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(304.8, 914.2, 608.6)]
    [TestCase(0.0, 914.2, 608.6)]
    [TestCase(304.8, 0.0, 608.6)]
    [TestCase(304.8, 914.2, 0)]
    [TestCase(0, 0, 608.6)]
    [TestCase(0, 914.2, 0)]
    [TestCase(304.8, 0, 0)]
    [TestCase(0, 0, 0)]
    public void LabwareEnvelope_Initialization_Test(decimal x, decimal y, decimal z)
    {
        // catch any dimensions that are 0 (less than zero are already caught by Millimeter and so won't be tested here)
        var length = new Mm(x);
        var width = new Mm(y);
        var height = new Mm(z);
        if (length.Equals(Mm.Zero) || width.Equals(Mm.Zero) || height.Equals(Mm.Zero))
        {
            Assert.Catch<LabwareEnvelopException>(() =>
            {
                var labwareEnvelope = new LabwareEnvelope(length, width, height);
            });
            Assert.Pass();
        }
        // ensure a successful initialization
        var labwareEnvelope = new LabwareEnvelope(length, width, height);
        // ensure properties are as expected
        Assert.That(length, Is.EqualTo(labwareEnvelope.LengthXMm));
        Assert.That(width, Is.EqualTo(labwareEnvelope.WidthYMm));
        Assert.That(height, Is.EqualTo(labwareEnvelope.HeightZMm));
    }
}