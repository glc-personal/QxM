namespace Qx.Infrastructure.Persistence.Entities;

// TODO: Make persistent and it's own thing (no history / maybe a shallow history)
public class LocationEntity
{
    public Guid Id { get; set; } // PK
    public string Name { get; set; } = null!;
    public bool IsCustom { get; set; }
    
    // Physical coordinates in a defined frame
    public double XUs { get; set; }
    public double YUs { get; set; }
    public double ZUs { get; set; }
    public string Frame { get; set; } = null!;
}