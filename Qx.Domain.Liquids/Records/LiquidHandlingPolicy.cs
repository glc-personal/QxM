namespace Qx.Domain.Liquids.Records;

/// <summary>
/// Liquid handling policy for how to handle a liquid
/// </summary>
/// <param name="UsePreAirGap">Use an air gap before handling</param>
/// <param name="UsePostAirGap">Use an air gap after handling</param>
/// <param name="PreAirGapVolume">Pre air gap volume</param>
/// <param name="PostAirGapVolume">Post air gap volume</param>
/// <param name="OverAspirationVolume">Volume offset</param>
/// <param name="DefaultMixCycles">Number of mixing cycles</param>
public sealed record LiquidHandlingPolicy(bool UsePreAirGap,
    bool UsePostAirGap,
    Volume? PreAirGapVolume,
    Volume? PostAirGapVolume,
    Volume? OverAspirationVolume,
    uint DefaultMixCycles);