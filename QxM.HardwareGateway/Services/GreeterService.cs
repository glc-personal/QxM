using Grpc.Core;
using QxM.HardwareGateway;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Infrastructure.Simulators;

namespace QxM.HardwareGateway.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private SimulatedIcbClient _icb;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
        var tp = new TimeoutPolicy(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), 
            TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        _icb = new SimulatedIcbClient(tp);
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        Console.WriteLine($"{_icb.HardwareKind} Connection State: {_icb.ConnectionState}");
        _icb.ConnectAsync().Wait();
        Console.WriteLine($"{_icb.HardwareKind} Connection State: {_icb.ConnectionState}");
        var commandRequest = HardwareCommandRequest.Create(IdempotencyKey.New(), CorrelationId.New(), new Address(1),
            "mabs", new ReadOnlyMemory<byte>([1, 2, 3]), _icb.TimeoutPolicy.CommandTimeout);
        var response = _icb.ExecuteCommandAsync(commandRequest).Result;
        Console.WriteLine($"{response.CommandId}");
        Console.WriteLine($"{response.Status}");
        Console.WriteLine($"{response.IsTerminal}");
        Console.WriteLine($"{response.Payload}");
        Console.WriteLine($"{response.Error}");
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello from {_icb.HardwareKind}"
        });
    }
}