using Grpc.Core;
using QxM.HardwareGateway.Application;
using QxM.HardwareGateway.Application.Simulators;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Can;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Infrastructure.Simulators;

namespace QxM.HardwareGateway.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private SimulatedIcbClient _icb;
    private SimulatedPipettorClient _pipettor;
    private SimulatedIcbAdapter _icbAdapater;
    private SimulatedPipettorAdapter _pipettorAdapter;
    private HardwareGatewayRouter _router;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
        
        var tp = new TimeoutPolicy(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), 
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        
        _icb = new SimulatedIcbClient(tp);
        _pipettor = new SimulatedPipettorClient(tp);

        _icb.ConnectAsync().GetAwaiter().GetResult();
        _pipettor.ConnectAsync().GetAwaiter().GetResult();
        
        _icbAdapater = new SimulatedIcbAdapter(_icb);
        _pipettorAdapter = new SimulatedPipettorAdapter(_pipettor);

        _router = new HardwareGatewayRouter(new List<IHardwareAdapter>
        {
            _icbAdapater,
            _pipettorAdapter,
        });
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var scheme = new HardwareGatewayRoutingScheme(_icb.HardwareId, _icb.HardwareKind);
        var envelopeRequest = new HardwareGatewayCommandRequestEnvelope(IdempotencyKey.New(), CorrelationId.New(),
            new Address(1), "mabs", new ReadOnlyMemory<byte>([1, 2, 3, 4]),
            new TimeSpan(0, 0, 0, 5));
        var responseEnvelope = _router.RouteExecuteCommandAsync(scheme, 
            envelopeRequest).Result;
        
        Console.WriteLine(scheme.HardwareKind);
        Console.WriteLine($"Response Command Id: {responseEnvelope.CommandId}");
        Console.WriteLine($"Response Command Status: {responseEnvelope.CommandStatus}");
        Console.WriteLine($"Response Command Error: {responseEnvelope.Error}");
        Console.WriteLine($"Response Command Payload: {responseEnvelope.Payload}");
        
        scheme = new HardwareGatewayRoutingScheme(_pipettor.HardwareId, _pipettor.HardwareKind);
        envelopeRequest = new HardwareGatewayCommandRequestEnvelope(IdempotencyKey.New(), CorrelationId.New(),
            null, "aspirate", new ReadOnlyMemory<byte>([100,100,100,100,100,100,100,100]),
            new TimeSpan(0, 0, 0, 20));
        responseEnvelope = _router.RouteExecuteCommandAsync(scheme, 
            envelopeRequest).Result;
        
        Console.WriteLine(scheme.HardwareKind);
        Console.WriteLine($"Response Command Id: {responseEnvelope.CommandId}");
        Console.WriteLine($"Response Command Status: {responseEnvelope.CommandStatus}");
        Console.WriteLine($"Response Command Error: {responseEnvelope.Error}");
        Console.WriteLine($"Response Command Payload: {responseEnvelope.Payload}");
        
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello"
        });
    }
}