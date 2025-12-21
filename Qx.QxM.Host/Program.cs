using Qx.Application.Services.Implementations;
using Qx.Application.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// TODO: Abstract away Serilog with Qx.Core.Logging ...
#region LoggingProvider
// setup the Serilog logger from settings
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // reads the Serilog section in appsettings.json
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// scoped services: those that are unit-of-work per request, use a DbContext, ect
#region ScopedServices
builder.Services.AddScoped<IInventoryAppService, InventoryAppService>();
#endregion
// singleton services: 
#region SingletonServices
//
#endregion
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();