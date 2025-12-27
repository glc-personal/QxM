using Qx.Domain.Consumables.Enums;
using Qx.Domain.Consumables.Implementations;

namespace Qx.Application.Services.Dtos;

public sealed class ConsumableDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ConsumableTypeDto Type { get; set; } = null!;
    public ConsumableStates State { get; set; }
    public bool IsReusable { get; set; }
    public int? MaxUses { get; set; }
    public int Uses { get; set; }
    public bool IsSealed { get; set; }
    public LocationDto Location { get; set; } = null!;
    public IReadOnlyList<ConsumableColumnDto> Columns { get; init; } = Array.Empty<ConsumableColumnDto>();
}