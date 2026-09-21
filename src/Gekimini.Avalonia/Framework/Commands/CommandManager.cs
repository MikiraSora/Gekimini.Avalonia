using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Gekimini.Avalonia.Framework.Commands;

/// <summary>
/// Broadcasts command requery requests to <see cref="RequerySuggested"/> subscribers.
/// A pass re-evaluates command state on the UI thread at <see cref="DispatcherPriority.Input"/>
/// and runs at most once per <see cref="ThrottleMilliseconds"/>, so a burst of input or state
/// changes collapses into a single pass. Subscriptions are held weakly: a command binding does
/// not keep its owner alive, and dead subscriptions are dropped while a pass runs.
/// </summary>
public static class CommandManager
{
    private const int ThrottleMilliseconds = 100;

    // Copy-on-write list: a pass notifies the array it captured, so subscribing or unsubscribing
    // from inside a subscriber (menu rebuilds do that) cannot disturb the running pass.
    private static readonly Lock subscriptionLock = new();
    private static volatile WeakReference<EventHandler>[] subscriptions = [];

    private static int isPassScheduled;
    private static long lastPassTimestamp;

    static CommandManager()
    {
        InputElement.GotFocusEvent.AddClassHandler<InputElement>((_, _) => InvalidateRequerySuggested("gotFocus"));
        InputElement.KeyDownEvent.AddClassHandler<InputElement>(
            (_, _) => InvalidateRequerySuggested("keyDown"),
            RoutingStrategies.Tunnel);
        InputElement.LostFocusEvent.AddClassHandler<InputElement>((_, _) => InvalidateRequerySuggested("lostFocus"));
        InputElement.PointerPressedEvent.AddClassHandler<InputElement>((_, _) => InvalidateRequerySuggested("pointerPressed"));
    }

    /// <summary>
    /// Raised on the UI thread for every requery pass. Handlers receive no event data: a pass is a
    /// state re-evaluation, not the input event that happened to trigger it.
    /// </summary>
    public static event EventHandler RequerySuggested
    {
        add => AddSubscription(value);
        remove => RemoveSubscription(value);
    }

    /// <summary>
    /// Requests a requery pass. <paramref name="reason"/> labels the trigger for diagnostics.
    /// Requests raised while a pass is already scheduled are collapsed into it, because the pass
    /// re-reads live command state instead of the state captured by the caller.
    /// </summary>
    public static void InvalidateRequerySuggested(string reason = "manualDefault")
    {
        if (Interlocked.CompareExchange(ref isPassScheduled, 1, 0) != 0)
            return;

        _ = RunPassAsync();
    }

    private static void AddSubscription(EventHandler value)
    {
        if (value is null)
            return;

        lock (subscriptionLock)
        {
            var current = subscriptions;
            foreach (var subscription in current)
            {
                if (subscription.TryGetTarget(out var existing) && existing == value)
                    return;
            }

            var updated = new WeakReference<EventHandler>[current.Length + 1];
            Array.Copy(current, updated, current.Length);
            updated[^1] = new WeakReference<EventHandler>(value);
            subscriptions = updated;
        }
    }

    private static void RemoveSubscription(EventHandler value)
    {
        lock (subscriptionLock)
        {
            var current = subscriptions;
            var kept = new List<WeakReference<EventHandler>>(current.Length);
            var removed = false;

            foreach (var subscription in current)
            {
                if (!subscription.TryGetTarget(out var existing))
                    continue; // dead subscriptions are dropped while we are rebuilding anyway

                // Subscriptions are deduplicated on add, so one match is the whole subscription.
                if (!removed && existing == value)
                {
                    removed = true;
                    continue;
                }

                kept.Add(subscription);
            }

            if (kept.Count != current.Length)
                subscriptions = kept.ToArray();
        }
    }

    private static async Task RunPassAsync()
    {
        try
        {
            var wait = ThrottleMilliseconds - ElapsedSinceLastPass();
            if (wait > 0)
                await Task.Delay((int)wait).ConfigureAwait(false);

            await Dispatcher.UIThread.InvokeAsync(NotifySubscribers, DispatcherPriority.Input);
            lastPassTimestamp = Stopwatch.GetTimestamp();
        }
        catch (Exception exception)
        {
            // The dispatcher can already be shutting down while a pass is queued.
            Trace.TraceError($"Command requery pass failed: {exception}");
        }
        finally
        {
            // Without this the scheduler would stay armed forever and requery would never run again.
            Volatile.Write(ref isPassScheduled, 0);
        }
    }

    private static long ElapsedSinceLastPass()
    {
        var previous = lastPassTimestamp;
        if (previous == 0)
            return long.MaxValue;

        // Monotonic clock: wall-clock adjustments must not stall or shorten the throttle window.
        return (Stopwatch.GetTimestamp() - previous) * 1000 / Stopwatch.Frequency;
    }

    private static void NotifySubscribers()
    {
        var current = subscriptions;
        var hasDeadSubscriptions = false;

        foreach (var subscription in current)
        {
            if (!subscription.TryGetTarget(out var handler))
            {
                hasDeadSubscriptions = true;
                continue;
            }

            try
            {
                handler(null, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                // One broken subscriber must not cut the pass short for the remaining subscribers,
                // and must never leave the requery scheduler stuck.
                Trace.TraceError($"A RequerySuggested subscriber failed: {exception}");
            }
        }

        if (hasDeadSubscriptions)
            PruneDeadSubscriptions();
    }

    private static void PruneDeadSubscriptions()
    {
        lock (subscriptionLock)
        {
            var current = subscriptions;
            var live = new List<WeakReference<EventHandler>>(current.Length);
            foreach (var subscription in current)
            {
                if (subscription.TryGetTarget(out _))
                    live.Add(subscription);
            }

            if (live.Count != current.Length)
                subscriptions = live.ToArray();
        }
    }
}
