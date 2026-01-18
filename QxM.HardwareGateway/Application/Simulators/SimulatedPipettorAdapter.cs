using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application.Simulators;

public sealed class SimulatedPipettorAdapter(IHardwareClient<ApiCommandRequest> hardwareClient)
    : SimulatedHardwareAdapterBase<ApiCommandRequest>(hardwareClient)
{
    protected override ApiCommandRequest ConvertToHardwareSpecific(HardwareGatewayCommandRequestEnvelope envelope)
    {
        return new ApiCommandRequest(envelope.IdempotencyKey, envelope.CorrelationId, envelope.Operation,
            envelope.Payload, envelope.Timeout);
    }
}