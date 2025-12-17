using QxM;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// TODO: Abstract away Serilog using Qx.Core.Logging ...
#region LoggingProvider 
// setup the Serilog logger from settings
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // reads tje Serilog section in appsettings.json
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders(); // replace default logging providers with Serilog
builder.Logging.AddSerilog(Log.Logger, dispose: true);
#endregion

#region ServiceRegistration
builder.Services.AddHostedService<Worker>();
#endregion

var host = builder.Build();
host.Run();