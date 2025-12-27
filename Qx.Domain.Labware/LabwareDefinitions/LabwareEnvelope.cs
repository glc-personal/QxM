using Qx.Core.Measurement;
using Qx.Domain.Labware.Exceptions;

namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Coarse envelope used for fit/clearance/collision coarse checks
/// Convention: dimensions are axis-aligned in the labware local frame
/// </summary>
public sealed record LabwareEnvelope
{
    public LabwareEnvelope(Millimeters lengthXMm, Millimeters widthYMm, Millimeters heightZMm)
    {
        if (lengthXMm.Equals(Millimeters.Zero) || widthYMm.Equals(Millimeters.Zero) ||
            heightZMm.Equals(Millimeters.Zero))
            throw new LabwareEnvelopException(this);
        LengthXMm = lengthXMm;
        WidthYMm = widthYMm;
        HeightZMm = heightZMm;
    }
    
    public Millimeters LengthXMm { get; }
    public Millimeters WidthYMm { get; }
    public Millimeters HeightZMm { get; }
}