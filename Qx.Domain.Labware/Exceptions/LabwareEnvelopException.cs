using System.Text;
using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Labware.Exceptions;

public class LabwareEnvelopException(LabwareEnvelope labwareEnvelop) 
    : ArgumentOutOfRangeException(LabwareEnvelopException.BuildExceptionMessage(labwareEnvelop))
{
    private static string BuildExceptionMessage(LabwareEnvelope labwareEnvelop)
    {
        var validLength = labwareEnvelop.LengthXMm.Equals(Millimeters.Zero);
        var validWidth = labwareEnvelop.WidthYMm.Equals(Millimeters.Zero);
        var validHeight = labwareEnvelop.HeightZMm.Equals(Millimeters.Zero);

        var messageBuild = new StringBuilder();
        messageBuild.Append("Invalid labware envelop dimensions: ");
        if (!validLength)
            messageBuild.Append($"Length: {labwareEnvelop.LengthXMm.ToString()} ");
        if (!validWidth)
            messageBuild.Append($"Width: {labwareEnvelop.WidthYMm.ToString()} ");
        if (!validHeight)
            messageBuild.Append($"Height: {labwareEnvelop.HeightZMm.ToString()} ");
        return messageBuild.ToString();
    }
}