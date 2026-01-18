using QxM.HardwareGateway.Core.Can;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Infrastructure;

namespace QxM.HardwareGateway.Application.Simulators;

public sealed class SimulatedIcbAdapter(IHardwareClient<CanFrameCommandRequest> hardwareClient)
    : SimulatedHardwareAdapterBase<CanFrameCommandRequest>(hardwareClient)
{
    // TODO: Make these StartOfFrame, EndOfFrame, and IsExtended configurable since they are fixed for the board
    private readonly StartOfFrame _startOfFrame = new(">");
    private readonly EndOfFrame _endOfFrame = new("</");
    private readonly bool _isExtended = false;

    protected override CanFrameCommandRequest ConvertToHardwareSpecific(
        HardwareGatewayCommandRequestEnvelope envelope)
    {
        if (!envelope.Address.HasValue)
            throw new ArgumentException($"{nameof(SimulatedIcbAdapter)} requires an address value");
        return new CanFrameCommandRequest(envelope.IdempotencyKey, envelope.CorrelationId, 
            envelope.Address.Value, envelope.Operation, envelope.Payload, envelope.Timeout, _startOfFrame, 
            _endOfFrame, false, _isExtended);
    }
}