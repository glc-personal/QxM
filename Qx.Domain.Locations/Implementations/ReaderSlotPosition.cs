using Qx.Domain.Locations.Enums;

namespace Qx.Domain.Locations.Implementations;

public sealed record ReaderSlotPosition : Position
{
    public ReaderSlotPosition(ReaderSlotNames name, BatchNames batch)
    {
        Name = name;
        Batch = batch;
    }
    
    public ReaderSlotNames Name { get; init; }
    public BatchNames Batch { get; init; }
}