using Qx.Domain.Liquids.Enums;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Liquids.Implementations;

public class LiquidClass(
    Viscosity viscosity,
    SurfaceTension surfaceTension,
    VolatilityLevels volatilityLevel,
    bool foamsEasily,
    bool containsBiomaterial,
    LiquidHandlingPolicy handlingPolicy)
{
    public Viscosity Viscosity { get; } = viscosity;
    public SurfaceTension SurfaceTension { get; } = surfaceTension;
    public VolatilityLevels VolatilityLevel { get; } = volatilityLevel;
    public bool FoamsEasily { get; } = foamsEasily;
    public bool ContainsBiomaterial { get; } = containsBiomaterial;
    public LiquidHandlingPolicy HandlingPolicy { get; } = handlingPolicy;
}