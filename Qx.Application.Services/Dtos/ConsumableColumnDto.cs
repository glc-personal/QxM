namespace Qx.Application.Services.Dtos;

public sealed class ConsumableColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsReusable { get; set; }
    public int? MaxUses { get; set; }
    public bool IsSealed { get; set; }
    public IList<double> Volumes { get; set; }
    public IList<double> Capacities { get; set; }
}