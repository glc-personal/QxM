using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

public sealed class SimulatedIcbClient : SimulatedHardwareClientBase<CanFrameCommandRequest>
{
    public SimulatedIcbClient(TimeoutPolicy timeoutPolicy, SimulatedHardwarePolicy? simulatedHardwarePolicy = null, 
        IdempotencyPolicy? idempotencyPolicy = null) : base(timeoutPolicy, simulatedHardwarePolicy, idempotencyPolicy)
    {
    }
}