using System.Collections.Concurrent;
using System.Threading.Channels;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Core.State;
using QxM.HardwareGateway.Core.Utilities;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

public abstract class SimulatedHardwareClientBase<TRequest> : IHardwareClient<TRequest> where TRequest : HardwareCommandRequest
{
    private const ConnectionState InitialState = ConnectionState.Disconnected;
    
    private readonly FiniteStateMachine<ConnectionState, ConnectionTrigger> _fsm;
    private readonly Channel<HardwareEvent> _eventsChannel;
    
    private readonly ConcurrentDictionary<IdempotencyKey, (CommandId CommandId, DateTimeOffset ExpiresAt)> _idempotency = new();
    private readonly ConcurrentDictionary<CommandId, TaskCompletionSource<HardwareCommandResponse>> _completed = new();
    
    private readonly SimulatedHardwarePolicy _simulatedHardwarePolicy;
    private readonly IdempotencyPolicy _idempotencyPolicy;

    private readonly string _firmwareVersion;
    
    public SimulatedHardwareClientBase(TimeoutPolicy timeoutPolicy, 
        SimulatedHardwarePolicy? simulatedHardwarePolicy = null, IdempotencyPolicy? idempotencyPolicy = null)
    {
        TimeoutPolicy = timeoutPolicy;
        HardwareId = HardwareId.New;
        _simulatedHardwarePolicy = simulatedHardwarePolicy ?? SimulatedHardwarePolicy.Default;
        _idempotencyPolicy = idempotencyPolicy ?? IdempotencyPolicy.Default;
        _firmwareVersion = $"sim-{HardwareKind}-1.0.1.0";
        
        // set up an unbound channel for hardware events
        _eventsChannel = Channel.CreateUnbounded<HardwareEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
        });
        
        // set up possible transitions for the connection state FSM
        var transitions = ConnectionTransitionsUtility.BuildTransitions();
        var onTransition = new Dictionary<ConnectionState, Func<string?, CancellationToken, Task>>
        {
            [ConnectionState.Disconnected] = (_, _) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Disconnected}");
                return Task.CompletedTask;
            },
            [ConnectionState.Connecting] = async (_, ct) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Connecting}");
                await Task.Delay(_simulatedHardwarePolicy.ConnectLatency,  ct).ConfigureAwait(false);
                if (IsSimulatedFault())
                    await _fsm!.FireAsync(ConnectionTrigger.ConnectFailed, "Connection Faulted", ct).ConfigureAwait(false);
                else
                    await _fsm!.FireAsync(ConnectionTrigger.ConnectSucceeded, "Connected", ct).ConfigureAwait(false);
            },
            [ConnectionState.Connected] = (_, _) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Connected}");
                return Task.CompletedTask;
            },
            [ConnectionState.Disconnecting] = async (_, ct) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Disconnecting}");
                await Task.Delay(_simulatedHardwarePolicy.DisconnectLatency,  ct).ConfigureAwait(false);
                if (IsSimulatedFault())
                    await _fsm!.FireAsync(ConnectionTrigger.DisconnectFailed, "Connection Faulted", ct).ConfigureAwait(false);
                else
                    await _fsm!.FireAsync(ConnectionTrigger.DisconnectSucceeded, "Disconnected", ct).ConfigureAwait(false);
            },
            [ConnectionState.Faulted] = (_, _) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Faulted}");
                return Task.CompletedTask;
            }
        };

        // set up the finite state machine
        _fsm = new FiniteStateMachine<ConnectionState, ConnectionTrigger>(InitialState, transitions, onTransition);
    }
    
    public HardwareId HardwareId { get; }
    public HardwareKind HardwareKind => HardwareKind.Pipettor;
    public ConnectionState ConnectionState => _fsm.State;
    public TimeoutPolicy TimeoutPolicy { get; }
    
    public virtual async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.ConnectTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
        
        await _fsm.FireAsync(ConnectionTrigger.Connect, "Connected", cancellationToken).ConfigureAwait(false);
        await Task.Delay(_simulatedHardwarePolicy.ConnectLatency, linkedCts.Token).ConfigureAwait(false);
        await _fsm.WaitForStateAsync(cs => cs is ConnectionState.Connected or ConnectionState.Faulted,
            TimeoutPolicy.ConnectTimeout, linkedCts.Token).ConfigureAwait(false);
        
        if (_fsm.State is ConnectionState.Faulted)
            throw new InvalidOperationException($"Simulated {HardwareKind} connection faulted");
    }

    public virtual async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.DisconnectTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
        
        await _fsm.FireAsync(ConnectionTrigger.Disconnect, "Disconnected", cancellationToken).ConfigureAwait(false);
        await Task.Delay(_simulatedHardwarePolicy.DisconnectLatency, linkedCts.Token).ConfigureAwait(false);
        await _fsm.WaitForStateAsync(cs => cs is ConnectionState.Disconnected or ConnectionState.Faulted,
            TimeoutPolicy.DisconnectTimeout, linkedCts.Token).ConfigureAwait(false);
        
        if (_fsm.State is ConnectionState.Faulted)
            throw new InvalidOperationException($"Simulated {HardwareKind} disconnection faulted");
    }

    public virtual async Task<HardwareHeartbeat> GetHeartbeatAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.HeartbeatTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
        
        var start = DateTimeOffset.UtcNow;
        // simulate heartbeat checks (e.g. Version)
        await Task.Delay(_simulatedHardwarePolicy.HeartbeatLatency, linkedCts.Token).ConfigureAwait(false);
        var roundTripTime = DateTimeOffset.UtcNow - start;

        var heartbeat = _fsm.State switch
        {
            ConnectionState.Disconnected => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
                HeartbeatStatus.Unreachable, roundTripTime, ConnectionState.Disconnected, _firmwareVersion, null),
            
            ConnectionState.Connected => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
                HeartbeatStatus.Ok, roundTripTime, ConnectionState.Connected, _firmwareVersion, null),
            
            ConnectionState.Connecting or ConnectionState.Disconnecting => new HardwareHeartbeat(HardwareId, 
                HardwareKind, DateTimeOffset.UtcNow, HeartbeatStatus.Ok, roundTripTime, ConnectionState, 
                _firmwareVersion, null),
            
            _ => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
            HeartbeatStatus.Faulted, roundTripTime, ConnectionState.Faulted,  _firmwareVersion, 
            new HardwareError("FAULTED", $"{HardwareKind} (Simulated): is {ConnectionState.Faulted}"))
        };

        _eventsChannel.Writer.TryWrite(new HardwareHeartbeatEvent(heartbeat.HeartbeatTimeStampUtc, HardwareId,
            HardwareKind, heartbeat));
        return heartbeat;
    }

    public virtual async Task<HardwareCommandResponse> ExecuteCommandAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        
        _completed.TryAdd(request.Id, 
            new TaskCompletionSource<HardwareCommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously));
        
        var cts = new CancellationTokenSource(TimeoutPolicy.CommandTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
        
        // submit the request
        await SubmitCommandAsync(request, linkedCts.Token).ConfigureAwait(false);
        
        // wait till the command has been executed successfully
        var tcs = _completed[request.Id];
        try
        {
            await Task.Delay(_simulatedHardwarePolicy.ExecuteLatency, linkedCts.Token).ConfigureAwait(false);
            return await tcs.Task.WaitAsync(linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested
                                                 && !cancellationToken.IsCancellationRequested)
        {
            var hardwareError = new HardwareError("TIMEOUT", "Command timed out");
            var timedOut =
                new HardwareCommandResponse(request.Id, CommandStatus.TimedOut, hardwareError, request.Payload);
            await EmitCommandLifecycleEvent("Command timed out", request.CorrelationId, request.Address,
                    request.Id, request.IdempotencyKey, request.Operation, CommandStatus.TimedOut, hardwareError,
                    linkedCts.Token)
                .ConfigureAwait(false);

            tcs.TrySetResult(timedOut);
            await Task.Delay(_simulatedHardwarePolicy.ExecuteLatency, linkedCts.Token).ConfigureAwait(false);
            return timedOut;
        }
        catch (OperationCanceledException)
        {
            var cancelled = new HardwareCommandResponse(request.Id, CommandStatus.Cancelled, null, request.Payload);
            await EmitCommandLifecycleEvent("Command cancelled", request.CorrelationId, request.Address,
                    request.Id, request.IdempotencyKey, request.Operation, CommandStatus.Cancelled, null, linkedCts.Token)
                .ConfigureAwait(false);
            
            tcs.TrySetResult(cancelled);
            await Task.Delay(_simulatedHardwarePolicy.ExecuteLatency, linkedCts.Token).ConfigureAwait(false);
            return cancelled;
        }
    }

    public virtual async Task<HardwareCommandAccepted> SubmitCommandAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        CleanupIdempotency();
        
        if (_idempotency.TryGetValue(request.IdempotencyKey, out var idempotencyKey)
            && idempotencyKey.ExpiresAt > DateTimeOffset.UtcNow)
        {
            await EmitCommandLifecycleEvent($"Command {request.Id} submitted", request.CorrelationId, 
                    request.Address, request.Id, request.IdempotencyKey, request.Operation, CommandStatus.Accepted, 
                    null, cancellationToken)
                .ConfigureAwait(false);
            return new HardwareCommandAccepted(request.Id, DateTimeOffset.UtcNow);
        }
        
        var commandId = request.Id;
        _idempotency[request.IdempotencyKey] = (commandId, DateTimeOffset.UtcNow.Add(_idempotencyPolicy.Expiration));
        _completed.TryAdd(commandId, new TaskCompletionSource<HardwareCommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously));
        await Task.Delay(_simulatedHardwarePolicy.SubmitLatency, cancellationToken).ConfigureAwait(false);
        await EmitCommandLifecycleEvent($"Command {request.Id} submitted", request.CorrelationId, 
                request.Address, request.Id, request.IdempotencyKey, request.Operation, CommandStatus.Accepted, 
                null, cancellationToken)
            .ConfigureAwait(false);
        _ = RunCommandAsync(request, cancellationToken);
        return new HardwareCommandAccepted(request.Id, DateTimeOffset.UtcNow);
    }

    public virtual async IAsyncEnumerable<HardwareEvent> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        while (await _eventsChannel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (_eventsChannel.Reader.TryRead(out var hardwareEvent))
                yield return hardwareEvent;
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        await DisconnectAsync().ConfigureAwait(false);
        await _fsm.DisposeAsync().ConfigureAwait(false);
        _eventsChannel.Writer.TryComplete();
    }
    
    protected virtual void EnsureConnected()
    {
        if (_fsm.State != ConnectionState.Connected)
            throw new InvalidOperationException($"{HardwareKind} (Simulated): cannot handle commands while the state is {_fsm.State}");
    }
    
    protected virtual void CleanupIdempotency()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var kvp in _idempotency)
        {
            if (kvp.Value.ExpiresAt == now)
                _idempotency.TryRemove(kvp.Key, out _);
        }
    }

    protected virtual bool IsSimulatedFault()
        => (Random.Shared.NextDouble() < _simulatedHardwarePolicy.FailureRate);

    /// <summary>
    /// Emit a connection state changed event to the channel
    /// </summary>
    /// <param name="message"></param>
    protected virtual void EmitConnectionStateChangedEvent(string message)
    {
        _eventsChannel.Writer.TryWrite(new HardwareConnectionChangedEvent(DateTimeOffset.UtcNow, 
            HardwareId, HardwareKind, ConnectionState, message));
    }
    
    protected virtual Task EmitCommandLifecycleEvent(string message, CorrelationId? correlationId, Address? address,
        CommandId commandId, IdempotencyKey idempotencyKey, string operation, CommandStatus status, HardwareError? error,
        CancellationToken cancellationToken)
    {
        return _eventsChannel.Writer.WriteAsync(new HardwareCommandLifecycleEvent(DateTimeOffset.UtcNow,
                HardwareId, HardwareKind, correlationId, address, commandId, idempotencyKey, operation, status, error), 
            cancellationToken).AsTask();
    }
    
    protected virtual async Task RunCommandAsync(TRequest request, CancellationToken linkedToken)
    {
        try
        {
            await Task.Delay(_simulatedHardwarePolicy.ExecuteLatency, linkedToken).ConfigureAwait(false);
            HardwareCommandResponse response;

            // TODO: Add a REJECTED option if the request.Operation not in accepted operations for this client
            if (IsSimulatedFault())
                response = new HardwareCommandResponse(request.Id, CommandStatus.Failed,
                    new HardwareError("FAULTED", $"Simulated hardware failure"), request.Payload);
            else
                response = new HardwareCommandResponse(request.Id, CommandStatus.Completed,
                    null, request.Payload);

            await EmitCommandLifecycleEvent($"Running command {request.Id}", request.CorrelationId, request.Address,
                request.Id, request.IdempotencyKey, request.Operation, response.Status, response.Error, linkedToken);
            if (_completed.TryGetValue(request.Id, out var tcs))
                tcs.TrySetResult(response);
        }
        finally
        {
            _completed.TryRemove(request.Id, out _);
        }
    }
}
