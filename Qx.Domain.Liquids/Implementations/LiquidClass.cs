using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Implementations;

public sealed record LiquidClass(
    Viscosity Viscosity,
    SurfaceTension SurfaceTension,
    VolatilityLevels VolatilityLevel,
    bool FoamsEasily,
    bool ContainsBiomaterial,
    LiquidHandlingPolicy HandlingPolicy);