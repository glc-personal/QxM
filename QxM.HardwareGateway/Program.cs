using QxM.HardwareGateway.Infrastructure;
using QxM.HardwareGateway.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// HardwareGateway Configuration
builder.Configuration.AddJsonFile(
    path: "Config/hardwaregateway.json",
    optional: false,
    reloadOnChange: true);
builder.Services
    .AddOptions<GatewayCommunicationOptions>()
    .Bind(builder.Configuration.GetSection("GatewayCommunication"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<HardwareGatewayService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();