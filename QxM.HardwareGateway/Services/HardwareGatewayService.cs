using Grpc.Core;
using Microsoft.Extensions.Options;
using QxM.HardwareGateway.Application.Factories;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Services;

public class HardwareGatewayService : HardwareGateway.HardwareGatewayBase
{
    private readonly ILogger<HardwareGatewayService> _logger;
    private readonly IHardwareGateway _gateway;
    private readonly GatewayCommunicationOptions _options;

    public HardwareGatewayService(IOptions<GatewayCommunicationOptions> options, ILogger<HardwareGatewayService> logger)
    {
        _logger = logger;
        _options = options.Value;
        _gateway = HardwareGatewayFactory.Create(_options);
    }

    public override Task<SendCommandResponse> SendCommand(SendCommandRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SendCommandResponse
        {
            CommandId = $"Test: {_options}"
        });
    }
}