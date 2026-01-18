namespace QxM.HardwareGateway.Core.Policy;

public sealed record SimulatedHardwarePolicy
{
    public SimulatedHardwarePolicy(TimeSpan connectLatency, TimeSpan disconnectLatency, TimeSpan executeLatency,
        TimeSpan submitLatency, TimeSpan heartbeatLatency, double failureRate)
    {
        ConnectLatency = connectLatency;
        DisconnectLatency = disconnectLatency;
        ExecuteLatency = executeLatency;
        SubmitLatency = submitLatency;
        HeartbeatLatency = heartbeatLatency;
        FailureRate = failureRate;
    }
    
    public TimeSpan ConnectLatency { get; }
    public TimeSpan DisconnectLatency { get; }
    public TimeSpan ExecuteLatency { get; }
    public TimeSpan SubmitLatency { get; }
    public TimeSpan HeartbeatLatency { get; }
    public double FailureRate { get; }

    public static SimulatedHardwarePolicy Default => new SimulatedHardwarePolicy(
        new TimeSpan(0, 0, 0, 0, 30),
        new TimeSpan(0, 0, 0, 0, 30),
        new TimeSpan(0, 0, 0, 0, 280),
        new TimeSpan(0, 0, 0, 0, 70),
        new TimeSpan(0, 0, 0, 0, 100),
        0.05);
    
    private void EnforceValidFailureRate(double rate)
    {
        if (rate is < 0 or > 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(SimulatedHardwarePolicy)} {nameof(FailureRate)} must be between 0 and 1");
    }
}