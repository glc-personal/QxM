using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Tests.Domain.Labware;

[TestFixture]
public class LabwareGridTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(8, 12, 5.6, 8.2, 3.5)]
    [TestCase(0, 12, 5.6, 8.2, 3.5)]
    [TestCase(8, 0, 5.6, 8.2, 3.5)]
    [TestCase(0, 0, 5.6, 8.2, 3.5)]
    [TestCase(-8, 12, 5.6, 8.2, 3.5)]
    [TestCase(-8, -12, 5.6, 8.2, 3.5)]
    [TestCase(8, -12, 5.6, 8.2, 3.5)]
    public void LabwareGrid_Initialization_Tests(int rowCount, int columnCount, 
        decimal rowPitchMm, decimal columnPitchMm, decimal firstColumnOffsetMm)
    {
        var rowPitch = new Millimeters(rowPitchMm);
        var columnPitch = new Millimeters(columnPitchMm);
        var firstColumnOffset = new Millimeters(firstColumnOffsetMm);
        if (rowCount <= 0 || columnCount <= 0)
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var labwareGrid = new LabwareGrid(rowCount, columnCount,
                    rowPitch, columnPitch, firstColumnOffset);
            });
            Assert.Pass();
        }
        var labwareGrid = new LabwareGrid(rowCount, columnCount, rowPitch, columnPitch, firstColumnOffset);
        // ensure labware grid properties are as expected
        Assert.That(labwareGrid.RowCount, Is.EqualTo(rowCount));
        Assert.That(labwareGrid.ColumnCount, Is.EqualTo(columnCount));
        Assert.That(labwareGrid.RowPitchMm, Is.EqualTo(rowPitch));
        Assert.That(labwareGrid.ColumnPitchMm, Is.EqualTo(columnPitch));
        Assert.That(labwareGrid.FirstColumnOffsetMm, Is.EqualTo(firstColumnOffset));
    }
}