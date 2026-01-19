using QxM.HardwareGateway.Core;

namespace QxM.HardwareGateway.Application;

public readonly record struct HardwareGatewayRoutingScheme(HardwareId HardwareId, 
    HardwareKind HardwareKind);