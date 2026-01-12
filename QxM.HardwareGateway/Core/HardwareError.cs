namespace QxM.HardwareGateway.Core;

public readonly record struct HardwareError
{
    public string Code { get; }
    public string Message { get; }
    
    public HardwareError(string code, string message)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException($"Invalid {nameof(HardwareError)}: error code ({code}) is required");
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException($"Invalid {nameof(HardwareError)}: error message ({message}) is required");
        Code = code;
        Message = message;
    }
}