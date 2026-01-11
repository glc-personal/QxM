using Grpc.Core;
using QxM.HardwareGateway.Core;

namespace QxM.HardwareGateway.Services;

public class HardwareGatewayService : HardwareGateway.HardwareGatewayBase
{
    private readonly ILogger<HardwareGatewayService> _logger;
    private readonly IHardwareGateway _gateway;

    public HardwareGatewayService(ILogger<HardwareGatewayService> logger)
    {
        _logger = logger;
    }

    public override Task<SendCommandResponse> SendCommand(SendCommandRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SendCommandResponse
        {
            CommandId = $"Test:"
        });
    }
}