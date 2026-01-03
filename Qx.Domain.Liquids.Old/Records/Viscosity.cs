using Qx.Domain.Liquids.Enums;

namespace Qx.Domain.Liquids.Records;

public sealed record Viscosity(double Value, ViscosityLevels Level);