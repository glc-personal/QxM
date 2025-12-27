using Qx.Core;

namespace Qx.Domain.Labware.LabwareInstances;

public sealed class Labware : IUniquelyIdentifiable
{
    public Guid Id { get; }
}