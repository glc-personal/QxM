using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Infrastructure;
using QxM.HardwareGateway.Infrastructure.Simulators;

namespace QxM.HardwareGateway.Application.Factories;

public sealed class HardwareGatewayFactory
{
    public static IHardwareGateway Create(GatewayCommunicationOptions options)
    {
        var gatewayCommunication = (options.IsSimulated) 
            ? SimulatedGatewayCommunication.Create(options)
            : GatewayCommunication.Create(options);
        return new HardwareGateway(gatewayCommunication);
    }
}