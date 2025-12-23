using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Qx.Infrastructure.Persistence.Contexts;

namespace Qx.Infrastructure.Persistence.Factories;

public class QxMDbContextFactory : IDesignTimeDbContextFactory<QxMDbContext>
{
    public QxMDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<QxMDbContext>();

        optionsBuilder.UseNpgsql(
            "host=localhost;Port=5432;Database=qxm_ics;Username=postgres;Password=password1234");
        return new QxMDbContext(optionsBuilder.Options);
    }
}