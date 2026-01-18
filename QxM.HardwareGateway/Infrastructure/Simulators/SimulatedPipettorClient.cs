using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

public sealed class SimulatedPipettorClient : SimulatedHardwareClientBase<ApiCommandRequest>
{
    public SimulatedPipettorClient(TimeoutPolicy timeoutPolicy, SimulatedHardwarePolicy? simulatedHardwarePolicy = null, 
        IdempotencyPolicy? idempotencyPolicy = null) 
        : base(HardwareKind.Pipettor, timeoutPolicy, simulatedHardwarePolicy, idempotencyPolicy)
    {
    }
}