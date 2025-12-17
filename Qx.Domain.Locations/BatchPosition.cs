using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations;

/// <summary>
/// Batch position on the work-deck.
/// </summary>
/// <param name="BatchName">Batch name of the position</param>
public sealed record BatchPosition(BatchNames BatchName) : Position;