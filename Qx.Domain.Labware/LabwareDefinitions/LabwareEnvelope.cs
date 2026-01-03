using Qx.Core.Measurement;
using Qx.Domain.Labware.Exceptions;

namespace Qx.Domain.Labware.LabwareDefinitions;

/// <summary>
/// Coarse envelope used for fit/clearance/collision coarse checks
/// Convention: dimensions are axis-aligned in the labware local frame
/// </summary>
public sealed record LabwareEnvelope
{
    public LabwareEnvelope(Mm lengthXMm, Mm widthYMm, Mm heightZMm)
    {
        if (lengthXMm.Equals(Mm.Zero) || widthYMm.Equals(Mm.Zero) ||
            heightZMm.Equals(Mm.Zero))
            throw new LabwareEnvelopException(this);
        LengthXMm = lengthXMm;
        WidthYMm = widthYMm;
        HeightZMm = heightZMm;
    }
    
    public Mm LengthXMm { get; }
    public Mm WidthYMm { get; }
    public Mm HeightZMm { get; }
}