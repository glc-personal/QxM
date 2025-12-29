using Qx.Domain.Labware.Policies;

namespace Qx.Domain.Labware.Models;

// Domain invariant: well specs within a column are always identical (this is why we have one well definition and not one per well)
public sealed record WellColumnDefinition(int ColumnIndex, WellDefinition WellDefinition, SealPolicy SealPolicy);