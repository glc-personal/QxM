using Qx.Domain.Liquids.Enums;

namespace Qx.Domain.Liquids.Records;

public sealed record SurfaceTension(double Value, SurfaceTensionLevels Level);