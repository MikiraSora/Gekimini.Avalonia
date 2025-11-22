using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Gekimini.Avalonia.Framework.Commands;

public static class CommandManager
{
    private static bool isBroadcastEvent;
    private static readonly List<WeakReference<EventHandler>> registeredHandlers = [];
    private static readonly HashSet<WeakReference<EventHandler>> removeItems = [];

    static CommandManager()
    {
        InputElement.GotFocusEvent.AddClassHandler<InputElement>(GotFocusEventHandler);
        InputElement.KeyDownEvent.AddClassHandler<InputElement>(KeyDownEventHandler, RoutingStrategies.Tunnel);
        InputElement.LostFocusEvent.AddClassHandler<InputElement>(CommonEventHandler);
        InputElement.PointerPressedEvent.AddClassHandler<InputElement>(CommonEventHandler);
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

    public static void InvalidateRequerySuggested()
    {
        RaiseRequerySuggested(null, EventArgs.Empty);
    }

    private static void CommonEventHandler(InputElement targetElement, RoutedEventArgs args)
    {
        RaiseRequerySuggested(targetElement, args);
    }

    private static void GotFocusEventHandler(InputElement targetElement, GotFocusEventArgs args)
    {
        RaiseRequerySuggested(targetElement, args);
    }

    private static void KeyDownEventHandler(InputElement targetElement, KeyEventArgs inputEventArgs)
    {
        RaiseRequerySuggested(targetElement, inputEventArgs);
    }

    private static void RaiseRequerySuggested(InputElement sender, EventArgs args)
    {
        if (isBroadcastEvent)
            return;

        Dispatcher.UIThread.InvokeAsync(() => RaiseRequerySuggestedImpl(sender, args),
            DispatcherPriority.Background);
    }

    private static void RaiseRequerySuggestedImpl(InputElement sender, EventArgs args)
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