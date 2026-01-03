using Qx.Core.Measurement;
using Qx.Domain.Labware.LabwareDefinitions;
using Qx.Domain.Liquids.Records;

namespace Qx.Domain.Labware.Models;

public sealed record TipDefinition(TipCapacity Capacity, Volume MinimumRecommendedVolume, TipStyle Style, 
    Mm Length, Mm SeatedDepth, bool Filtered, bool Conductive);