using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

public sealed class SimulatedCameraClient : SimulatedHardwareClientBase<ApiCommandRequest>
{
    public SimulatedCameraClient(TimeoutPolicy timeoutPolicy, SimulatedHardwarePolicy? simulatedHardwarePolicy = null, 
        IdempotencyPolicy? idempotencyPolicy = null)
        : base(HardwareKind.Camera, timeoutPolicy, simulatedHardwarePolicy, idempotencyPolicy)
    {
    }
}