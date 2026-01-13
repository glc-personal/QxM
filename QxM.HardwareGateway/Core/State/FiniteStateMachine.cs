using System.Threading.Channels;

namespace QxM.HardwareGateway.Core.State;

public sealed class FiniteStateMachine<TState, TTrigger> 
    : IAsyncDisposable where TState : Enum where TTrigger : Enum
{
    private readonly IReadOnlyDictionary<(TState, TTrigger), TState> _transitions;
    private readonly IReadOnlyDictionary<TState, Func<string?, CancellationToken, Task>> _onTransition;
    private readonly Channel<TriggerMessage<TTrigger>> _triggerMessageChannel;
    private readonly Task _pumpTask;
    private readonly CancellationTokenSource _cts = new();
    private TState _currentState;

    public FiniteStateMachine(TState initialState, IEnumerable<Transition<TState, TTrigger>> transitions,
        IReadOnlyDictionary<TState, Func<string?, CancellationToken, Task>>? onTransition = null)
    {
        _currentState = initialState;
        _transitions = transitions.ToDictionary(
            t => (t.From, t.Trigger),
            t => t.To);
        _onTransition = onTransition ?? new Dictionary<TState, Func<string?, CancellationToken, Task>>();
        
        _triggerMessageChannel = Channel.CreateUnbounded<TriggerMessage<TTrigger>>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        });
        
        _pumpTask = Task.Run(PumpAsync);
    }
    
    public TState State => _currentState;
    public event Action<StateChanged<TState, TTrigger>>? OnStateChanged;

    /// <summary>
    /// Fire a trigger for a state transition.
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task FireAsync(TTrigger trigger, string? reason = null, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled(cancellationToken);
        
        // add fired trigger to the channel
        var triggerMessage = new TriggerMessage<TTrigger>(trigger, reason);
        if (!_triggerMessageChannel.Writer.TryWrite(triggerMessage))
            throw new InvalidOperationException($"{nameof(FiniteStateMachine<TState, TTrigger>)} is not accepting triggers.");
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Wait for a particular state
    /// </summary>
    /// <param name="predicate">Predicate function to filter on which state(s) to wait for</param>
    /// <param name="timeout">Timeout to wait for the state</param>
    /// <param name="cancellationToken"></param>
    public async Task WaitForStateAsync(Func<TState, bool> predicate, TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        if (predicate(_currentState))
            return;
        
        // set up a cancellation token source for the timeout
        using var cts = new CancellationTokenSource(timeout);
        // set up a linked cancellation token source for a general cancellation and the timeout cancellation
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        
        // allow rechecks of the predicate on any state change
        void Handler(StateChanged<TState, TTrigger> _)
        {
            if (predicate(_currentState))
                tcs.TrySetResult();
        }
        
        OnStateChanged += Handler;
        try
        {
            if (predicate(_currentState))
                return;
            await tcs.Task.WaitAsync(linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException($"{nameof(FiniteStateMachine<TState, TTrigger>)} timed out waiting for state predicate. Current state: {_currentState}");
        }
        finally
        {
            OnStateChanged -= Handler;
        }
    }

    /// <summary>
    /// Pump to keep the Finite State Machine engine going
    /// </summary>
    private async Task PumpAsync()
    {
        try
        {
            while (await _triggerMessageChannel.Reader.WaitToReadAsync(_cts.Token).ConfigureAwait(false))
            {
                while (_triggerMessageChannel.Reader.TryRead(out var triggerMessage))
                {
                    // trigger the state change
                    ApplyTrigger(triggerMessage.Trigger, triggerMessage.Reason);

                    if (_onTransition.TryGetValue(_currentState, out var onTransition))
                        await onTransition(triggerMessage.Reason, _cts.Token).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // normal shutdown
        }
        catch (Exception exception)
        {
            // TODO: Log this
            Console.WriteLine($"{nameof(FiniteStateMachine<TState, TTrigger>)} failed with {exception.Message}");
        }
    }

    /// <summary>
    /// Trigger the state change if it is valid
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="reason"></param>
    private void ApplyTrigger(TTrigger trigger, string? reason)
    {
        var from = _currentState;

        if (!_transitions.TryGetValue((from, trigger), out var to))
            return; // ignore invalid triggers
        
        _currentState = to;
        OnStateChanged?.Invoke(new StateChanged<TState, TTrigger>(from, to, trigger, reason));
    }
    
    public async ValueTask DisposeAsync()
    {
        await _cts.CancelAsync();
        _triggerMessageChannel.Writer.TryComplete();
        try
        {
            await _pumpTask.ConfigureAwait(false);
        }
        catch
        {
            // ignore
        }
        _cts.Dispose();
    }
}