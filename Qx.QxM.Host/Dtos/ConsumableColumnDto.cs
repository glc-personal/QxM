namespace Qx.QxM.Host.Dtos;

public sealed class ConsumableColumnDto
{
    public Guid Id { get; set; }
    public bool IsSealed { get; set; }
    public bool IsReusable { get; set; }
    public int? MaxUses { get; set; }
    public int Uses { get; set; }
    public IReadOnlyList<double> VolumesUl { get; init; } = Array.Empty<double>();
    public IReadOnlyList<double> CapacitiesUl { get; init; } = Array.Empty<double>();
}