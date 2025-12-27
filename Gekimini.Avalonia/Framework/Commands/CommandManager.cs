using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Gekimini.Avalonia.Framework.Commands;

public static class CommandManager
{
    private static DateTime prevTime;
    private static volatile bool isBroadcastEvent;
    private static readonly List<WeakReference<EventHandler>> registeredHandlers = [];
    private static readonly HashSet<WeakReference<EventHandler>> removeItems = [];

    static CommandManager()
    {
        InputElement.GotFocusEvent.AddClassHandler<InputElement>(GotFocusEventHandler);
        InputElement.KeyDownEvent.AddClassHandler<InputElement>(KeyDownEventHandler, RoutingStrategies.Tunnel);
        InputElement.LostFocusEvent.AddClassHandler<InputElement>((s, e) => CommonEventHandler(s, e, "lostFocus"));
        InputElement.PointerPressedEvent.AddClassHandler<InputElement>((s, e) =>
            CommonEventHandler(s, e, "pointerPressed"));
    }

    public static event EventHandler RequerySuggested
    {
        add => AddHandler(value);
        remove => RemoveHandler(value);
    }

    private static void RemoveHandler(EventHandler value)
    {
        registeredHandlers.RemoveAll(x => !x.TryGetTarget(out var handler) || handler == value);
    }

    private static void AddHandler(EventHandler value)
    {
        if (registeredHandlers.Any(x => x.TryGetTarget(out var handler) && handler == value))
            //duplicated, skipped.
            return;

        registeredHandlers.Add(new WeakReference<EventHandler>(value));
    }

    public static void InvalidateRequerySuggested(string reason = "manualDefault")
    {
        RaiseRequerySuggested(null, EventArgs.Empty, reason);
    }

    private static void CommonEventHandler(InputElement targetElement, RoutedEventArgs args, string reason)
    {
        RaiseRequerySuggested(targetElement, args, reason);
    }

    private static void GotFocusEventHandler(InputElement targetElement, GotFocusEventArgs args)
    {
        RaiseRequerySuggested(targetElement, args, "gotFocus");
    }

    private static void KeyDownEventHandler(InputElement targetElement, KeyEventArgs inputEventArgs)
    {
        RaiseRequerySuggested(targetElement, inputEventArgs, "keyDown");
    }

    private static async void RaiseRequerySuggested(InputElement sender, EventArgs args, string reason,
        bool force = false)
    {
        if (isBroadcastEvent && !force)
            return;
        isBroadcastEvent = true;

        var curTime = DateTime.UtcNow;
        var pastTime = curTime - prevTime;
        if (pastTime.TotalMilliseconds <= 100)
            await Task.Delay(TimeSpan.FromMilliseconds(100 - pastTime.TotalMilliseconds));

        await Dispatcher.UIThread.InvokeAsync(() => RaiseRequerySuggestedImpl(sender, args, reason),
            DispatcherPriority.Input);
        prevTime = DateTime.UtcNow;
    }

    private static void RaiseRequerySuggestedImpl(InputElement sender, EventArgs args, string reason)
    {
        foreach (var registeredHandler in registeredHandlers)
            if (registeredHandler.TryGetTarget(out var handler))
                handler.Invoke(sender, args);
            else
                removeItems.Add(registeredHandler);

        foreach (var removeItem in removeItems)
            registeredHandlers.Remove(removeItem);

        removeItems.Clear();
        isBroadcastEvent = false;
    }
}