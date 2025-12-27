using Qx.Domain.Labware.LabwareDefinitions;

namespace Qx.Domain.Labware.Models;

public sealed record WellDefinition(WellShape Shape, WellBottom Bottom, WellCapacity Capacity);