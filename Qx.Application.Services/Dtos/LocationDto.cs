namespace Qx.Application.Services.Dtos;

public sealed class LocationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double XUs { get; set; }
    public double YUs { get; set; }
    public double ZUs { get; set; }
    public string Frame { get; set; }
    public bool IsCustom { get; set; }
}