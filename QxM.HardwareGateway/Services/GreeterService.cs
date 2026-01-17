using Grpc.Core;
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
    private SimulatedIcbAdapter _icbAdapater;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
        var tp = new TimeoutPolicy(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), 
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        _icb = new SimulatedIcbClient(tp);
        _icbAdapater = new SimulatedIcbAdapter(_icb);
        
        Console.WriteLine($"{_icb.HardwareKind} Connection State: {_icb.ConnectionState}");
        _icb.ConnectAsync().Wait();
        Console.WriteLine($"{_icb.HardwareKind} Connection State: {_icb.ConnectionState}");
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var envelope = new HardwareGatewayCommandRequestEnvelope(IdempotencyKey.New(), CorrelationId.New(), new Address(1),
            "mabs", new ReadOnlyMemory<byte>([1,2,3,4]), _icb.TimeoutPolicy.CommandTimeout);
        var canFrameRequest = new CanFrameCommandRequest(IdempotencyKey.New(), CorrelationId.New(), new Address(1),
            "mabs", new ReadOnlyMemory<byte>([1,2,3,4]), _icb.TimeoutPolicy.CommandTimeout, new StartOfFrame(">"),
            new EndOfFrame("<"), false, false);
        var acceptedResponse = _icb.SubmitCommandAsync(canFrameRequest).Result;
        Console.WriteLine($"Accepted Response: {acceptedResponse.CommandId}");
        Console.WriteLine($"Accepted Response: {acceptedResponse.AcceptedAtUtc}");
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
        var events = _icb.SubscribeAsync(cts.Token);
        var bevents = events.ToBlockingEnumerable();
        foreach (var bevent in bevents)
        {
            try
            {
                var e = (HardwareCommandLifecycleEvent)bevent;
                Console.WriteLine($"Event: {e.TimestampUtc}");
                Console.WriteLine($"Event: {e.Status}");
                Console.WriteLine($"Event: {e.CommandId}");
                Console.WriteLine($"Event: {e.Error}");
            }
            catch
            {
                // ignored
            }
        }
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello from {_icbAdapater.HardwareKind}"
        });
    }
}