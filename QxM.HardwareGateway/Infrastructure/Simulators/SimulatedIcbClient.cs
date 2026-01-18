using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using QxM.HardwareGateway.Core;
using QxM.HardwareGateway.Core.Events;
using QxM.HardwareGateway.Core.Policy;
using QxM.HardwareGateway.Core.Requests;
using QxM.HardwareGateway.Core.Responses;
using QxM.HardwareGateway.Core.State;
using QxM.HardwareGateway.Core.Utilities;

namespace QxM.HardwareGateway.Infrastructure.Simulators;

public sealed class SimulatedIcbClient : IHardwareClient<CanFrameCommandRequest>, IAsyncDisposable
{
    private readonly FiniteStateMachine<ConnectionState, ConnectionTrigger> _fsm;
    private readonly Channel<HardwareEvent> _eventsChannel;

    private readonly ConcurrentDictionary<IdempotencyKey, (CommandId CommandId, DateTimeOffset ExpiresAt)> _idempotency = new();
    private readonly ConcurrentDictionary<CommandId, TaskCompletionSource<HardwareCommandResponse>> _completed = new();

    private readonly string _firmwareVersion = "sim-icb-1.0.1.0";
    
    private readonly TimeSpan _connectingLatency = TimeSpan.FromMilliseconds(500);
    private readonly TimeSpan _disconnectingLatency = TimeSpan.FromMilliseconds(300);
    private readonly TimeSpan _executeLatency = TimeSpan.FromMilliseconds(5180);
    private readonly TimeSpan _submitLatency = TimeSpan.FromMilliseconds(30);
    private readonly TimeSpan _heartbeatLatency = TimeSpan.FromMilliseconds(20);
    private readonly TimeSpan _idempotencyExpiration = TimeSpan.FromMilliseconds(500);

    private readonly double _failureRate = 0.05;

    public SimulatedIcbClient(TimeoutPolicy timeoutPolicy)
    {
        HardwareId = HardwareId.New;
        HardwareKind = HardwareKind.Icb;
        TimeoutPolicy = timeoutPolicy;

        // set up the unbounded channel for hardware events
        _eventsChannel = Channel.CreateUnbounded<HardwareEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
        });

        var initialConnectionState = ConnectionState.Disconnected;
        var transitions = ConnectionTransitionsUtility.BuildTransitions();
        
        // set up the on transitions 
        var onTransition = new Dictionary<ConnectionState, Func<string?, CancellationToken, Task>>
        {
            [ConnectionState.Disconnected] = (_, __) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Disconnected}");
                return Task.CompletedTask;
            },
            [ConnectionState.Connecting] = async (_, ct) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Connecting}");
                // simulate the connecting process
                await Task.Delay(_connectingLatency, ct).ConfigureAwait(false);
                await _fsm.FireAsync(ConnectionTrigger.ConnectSucceeded, "Connected", ct).ConfigureAwait(false);
            },
            [ConnectionState.Connected] = (_, __) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Connected}");
                return Task.CompletedTask;
            },
            [ConnectionState.Disconnecting] = async (_, ct) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Disconnecting}");
                // simulate the disconnecting process
                await Task.Delay(_disconnectingLatency, ct).ConfigureAwait(false);
                await _fsm.FireAsync(ConnectionTrigger.DisconnectSucceeded, "Disconnected", ct).ConfigureAwait(false);
            },
            [ConnectionState.Faulted] = (reason, __) =>
            {
                EmitConnectionStateChangedEvent($"{HardwareKind} (Simulated): {ConnectionState.Faulted} {reason}");
                return Task.CompletedTask;
            }
        };
        
        // set up the finite state machine
        _fsm = new FiniteStateMachine<ConnectionState, ConnectionTrigger>(initialConnectionState, transitions, onTransition);
    }
    
    public HardwareId HardwareId { get; }
    public HardwareKind HardwareKind { get; }
    public ConnectionState ConnectionState => _fsm.State;
    public TimeoutPolicy TimeoutPolicy { get; }

    /// <summary>
    /// Simulate connecting to the hardware
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.ConnectTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);

        // attempt to connect
        await _fsm.FireAsync(ConnectionTrigger.Connect, "Connect requested", linkedCts.Token);
        
        // wait till connected
        await _fsm.WaitForStateAsync(cs => cs == ConnectionState.Connected || cs == ConnectionState.Faulted,
            TimeoutPolicy.ConnectTimeout,
            linkedCts.Token).ConfigureAwait(false);
        
        if (_fsm.State == ConnectionState.Faulted)
            throw new InvalidOperationException($"Simulated {HardwareKind} connection faulted");
    }

    /// <summary>
    /// Simulate disconnecting from the hardware
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.DisconnectTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
        
        // attempt to disconnect
        await _fsm.FireAsync(ConnectionTrigger.Disconnect, "Disconnect requested", linkedCts.Token);
        
        // wait till disconnected
        await _fsm.WaitForStateAsync(cs => cs == ConnectionState.Disconnected || cs == ConnectionState.Faulted,
            TimeoutPolicy.DisconnectTimeout,
            linkedCts.Token).ConfigureAwait(false);
        
        if (_fsm.State == ConnectionState.Faulted)
            throw new InvalidOperationException($"Simulated {HardwareKind} disconnection faulted");
    }

    /// <summary>
    /// Get the heartbeat of the client
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HardwareHeartbeat> GetHeartbeatAsync(CancellationToken cancellationToken = default)
    {
        var cts = new CancellationTokenSource(TimeoutPolicy.HeartbeatTimeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
        
        var start = DateTimeOffset.UtcNow;
        // simulate heartbeat checks (version, etc.)
        await Task.Delay(_heartbeatLatency, linkedCts.Token).ConfigureAwait(false);
        var roundTripTime = DateTimeOffset.UtcNow - start;

        var heartbeat = _fsm.State switch
        {
            ConnectionState.Disconnected => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
                HeartbeatStatus.Unreachable, roundTripTime, ConnectionState, _firmwareVersion, null),

            ConnectionState.Connecting or ConnectionState.Disconnecting => new HardwareHeartbeat(HardwareId,
                HardwareKind,
                DateTimeOffset.UtcNow, HeartbeatStatus.Ok, roundTripTime, ConnectionState, _firmwareVersion, null),

            ConnectionState.Connected => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
                HeartbeatStatus.Ok, roundTripTime, ConnectionState, _firmwareVersion, null),

            _ => new HardwareHeartbeat(HardwareId, HardwareKind, DateTimeOffset.UtcNow,
                HeartbeatStatus.Faulted, roundTripTime, ConnectionState, _firmwareVersion,
                new HardwareError("FAULTED", $"{HardwareKind} (Simulated): is {ConnectionState.Faulted}"))
        };
        
        _eventsChannel.Writer.TryWrite(new HardwareHeartbeatEvent(DateTimeOffset.UtcNow,
            HardwareId, HardwareKind, heartbeat));
        return heartbeat;
    }

    /// <summary>
    /// Execute and wait for a command response
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HardwareCommandResponse> ExecuteCommandAsync(CanFrameCommandRequest request, CancellationToken cancellationToken = default)
    {
        EnsureConnected();

        _completed.TryAdd(request.Id, 
            new TaskCompletionSource<HardwareCommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously));
        
        using var cts = new CancellationTokenSource(TimeoutPolicy.CommandTimeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
        
        // submit the request 
        await SubmitCommandAsync(request, linkedCts.Token).ConfigureAwait(false);
        
        // wait till command has been executed successfully
        var tcs = _completed[request.Id];
        try
        {
            await Task.Delay(_executeLatency, linkedCts.Token).ConfigureAwait(false);
            return await tcs.Task.WaitAsync(linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested &&
                                                 !cancellationToken.IsCancellationRequested)
        {
            var hardwareError = new HardwareError("TIMEOUT", "Command timed out");
            var timedOut =
                new HardwareCommandResponse(request.Id, CommandStatus.TimedOut, hardwareError, request.Payload);
            await EmitCommandLifecycleEvent("Command timed out", request.CorrelationId, request.Address,
                    request.Id, request.IdempotencyKey, request.Operation, CommandStatus.TimedOut, hardwareError,
                    linkedCts.Token)
                .ConfigureAwait(false);

            tcs.TrySetResult(timedOut);
            await Task.Delay(_executeLatency, linkedCts.Token).ConfigureAwait(false);
            return timedOut;
        }
        catch (OperationCanceledException)
        {
            var cancelled = new HardwareCommandResponse(request.Id, CommandStatus.Cancelled, null, request.Payload);
            await EmitCommandLifecycleEvent("Command cancelled", request.CorrelationId, request.Address,
                request.Id, request.IdempotencyKey, request.Operation, CommandStatus.Cancelled, null, linkedCts.Token)
                .ConfigureAwait(false);
            
            tcs.TrySetResult(cancelled);
            await Task.Delay(_executeLatency, linkedCts.Token).ConfigureAwait(false);
            return cancelled;
        }
    }

    /// <summary>
    /// Submit and forget a command response
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<HardwareCommandAccepted> SubmitCommandAsync(CanFrameCommandRequest request, CancellationToken cancellationToken = default)
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
        _idempotency[request.IdempotencyKey] = (commandId, DateTimeOffset.UtcNow.Add(_idempotencyExpiration));
        _completed.TryAdd(commandId, new TaskCompletionSource<HardwareCommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously));
        await Task.Delay(_submitLatency, cancellationToken).ConfigureAwait(false);
        await EmitCommandLifecycleEvent($"Command {request.Id} submitted", request.CorrelationId, 
                request.Address, request.Id, request.IdempotencyKey, request.Operation, CommandStatus.Accepted, 
                null, cancellationToken)
            .ConfigureAwait(false);
        _ = RunCommandAsync(request, cancellationToken);
        return new HardwareCommandAccepted(request.Id, DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Subscribe to hardware events to stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<HardwareEvent> SubscribeAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await _eventsChannel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (_eventsChannel.Reader.TryRead(out var hardwareEvent))
                yield return hardwareEvent;
        }
    }

    /// <summary>
    /// Emit a connection state changed event
    /// </summary>
    /// <param name="message"></param>
    private void EmitConnectionStateChangedEvent(string message)
    {
        _eventsChannel.Writer.TryWrite(new HardwareConnectionChangedEvent(DateTimeOffset.UtcNow,
            HardwareId,  HardwareKind, ConnectionState, message));
    }

    private Task EmitCommandLifecycleEvent(string message, CorrelationId? correlationId, Address? address,
        CommandId commandId, IdempotencyKey idempotencyKey, string operation, CommandStatus status, HardwareError? error,
        CancellationToken cancellationToken)
    {
        return _eventsChannel.Writer.WriteAsync(new HardwareCommandLifecycleEvent(DateTimeOffset.UtcNow,
            HardwareId, HardwareKind, correlationId, address, commandId, idempotencyKey, operation, status, error), 
            cancellationToken).AsTask();
    }

    private void EnsureConnected()
    {
        if (_fsm.State != ConnectionState.Connected)
            throw new InvalidOperationException($"{HardwareKind} (Simulated): cannot handle commands while the state is {_fsm.State}");
    }

    /// <summary>
    /// Clean up the idempotency dictionary by removing keys that are expired.
    /// </summary>
    private void CleanupIdempotency()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var kvp in _idempotency)
        {
            if (kvp.Value.ExpiresAt == now)
                _idempotency.TryRemove(kvp.Key, out _);
        }
    }

    /// <summary>
    /// Task for running the <see cref="CanFrameCommandRequest"/> asynchronously with a simulated rate of failure
    /// </summary>
    /// <param name="request"></param>
    /// <param name="linkedToken"></param>
    private async Task RunCommandAsync(CanFrameCommandRequest request, CancellationToken linkedToken)
    {
        try
        {
            await Task.Delay(_executeLatency, linkedToken).ConfigureAwait(false);
            HardwareCommandResponse response;

            // TODO: Add a REJECTED option if the request.Operation not in accepted operations for this client
            if (Random.Shared.NextDouble() < _failureRate)
            {
                response = new HardwareCommandResponse(request.Id, CommandStatus.Failed,
                    new HardwareError("FAULTED", $"Simulated hardware failure"), request.Payload);
            }
            else
            {
                response = new HardwareCommandResponse(request.Id, CommandStatus.Completed,
                    null, request.Payload);
            }

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

    public async ValueTask DisposeAsync()
    {
        await _fsm.DisposeAsync().ConfigureAwait(false);
        _eventsChannel.Writer.TryComplete();
    }
}