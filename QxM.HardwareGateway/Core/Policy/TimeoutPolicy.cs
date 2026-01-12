namespace QxM.HardwareGateway.Core.Policy;

public sealed record TimeoutPolicy
{
    public TimeSpan ConnectTimeout { get; }
    public TimeSpan DisconnectTimeout { get; }
    public TimeSpan HeartbeatTimeout { get; }
    public TimeSpan CommandTimeout { get; }

    public TimeoutPolicy(TimeSpan connectTimeout, TimeSpan disconnectTimeout, TimeSpan heartbeatTimeout,
        TimeSpan commandTimeout)
    {
        EnforcePositiveTimeouts(connectTimeout, nameof(connectTimeout));
        EnforcePositiveTimeouts(disconnectTimeout, nameof(disconnectTimeout));
        EnforcePositiveTimeouts(heartbeatTimeout, nameof(heartbeatTimeout));
        EnforcePositiveTimeouts(commandTimeout, nameof(commandTimeout));
        ConnectTimeout = connectTimeout;
        DisconnectTimeout = disconnectTimeout;
        HeartbeatTimeout = heartbeatTimeout;
        CommandTimeout = commandTimeout;
    }

    private void EnforcePositiveTimeouts(TimeSpan timeout, string timeoutName)
    {
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException($"Invalid {nameof(TimeoutPolicy)}: {timeoutName} must be positive");
    }
}