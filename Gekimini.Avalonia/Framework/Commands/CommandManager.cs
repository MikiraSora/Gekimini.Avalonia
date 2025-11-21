//code copied&modified from https://github.com/AvaloniaUI/Avalonia.Labs/blob/main/src/Avalonia.Labs.CommandManager/CommandManager.cs
/*
MIT License

Copyright (c) 2023 AvaloniaUI

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Utilities;

namespace Gekimini.Avalonia.Framework.Commands;

public sealed class CommandManager : AvaloniaObject
{
    private static CommandManager s_commandManager;

    private static readonly WeakReference<IInputElement> s_inputElement = new(default);

    private static readonly WeakEvent<CommandManager, EventArgs> PrivateRequerySuggestedEvent =
        WeakEvent.Register<CommandManager>(
            (m, v) => { m.PrivateRequerySuggested += v; },
            (m, v) => { m.PrivateRequerySuggested -= v; });

    private DispatcherOperation _requerySuggestedOperation;

    static CommandManager()
    {
        InputElement.GotFocusEvent.AddClassHandler<InputElement>(GotFocusEventHandler);
        InputElement.KeyDownEvent.AddClassHandler<InputElement>(KeyDownEventHandler, RoutingStrategies.Tunnel);
        InputElement.LostFocusEvent.AddClassHandler<InputElement>(CommonEventHandler);
        //we need this?
        InputElement.PointerPressedEvent.AddClassHandler<InputElement>(CommonEventHandler);
    }

    private CommandManager()
    {
    }

    private static CommandManager Current => s_commandManager ??= new CommandManager();

    private event EventHandler PrivateRequerySuggested;

    /// <summary>
    ///     Occurs when the <see cref="CommandManager" /> detects conditions that might change the ability of a command to
    ///     execute.
    /// </summary>
    public static event EventHandler RequerySuggested
    {
        // WeakHandlerWrapper will ensure, that add/remove with the same handler will work.
        add => PrivateRequerySuggestedEvent.Subscribe(Current, new WeakHandlerWrapper(value));
        remove => PrivateRequerySuggestedEvent.Unsubscribe(Current, new WeakHandlerWrapper(value));
    }

    /// <summary>
    ///     Invokes RequerySuggested listeners registered on the current thread.
    /// </summary>
    public static void InvalidateRequerySuggested()
    {
        Current.RaiseRequerySuggested();
    }

    private static void CommonEventHandler(InputElement targetElement, RoutedEventArgs args)
    {
        s_inputElement.SetTarget(args.Source as IInputElement);
        Current.RaiseRequerySuggested();
    }

    private static void GotFocusEventHandler(InputElement targetElement, GotFocusEventArgs args)
    {
        s_inputElement.SetTarget(args.Source as IInputElement);
        Current.RaiseRequerySuggested();
    }

    private static void KeyDownEventHandler(InputElement targetElement, KeyEventArgs inputEventArgs)
    {
        s_inputElement.SetTarget(inputEventArgs.Source as IInputElement);
        Current.RaiseRequerySuggested();
    }

    private void RaiseRequerySuggested()
    {
        if (_requerySuggestedOperation == null)
        {
            var dispatcher = Dispatcher.UIThread; // should be CurrentDispatcher
            _requerySuggestedOperation =
                dispatcher.InvokeAsync(RaiseRequerySuggestedImpl, DispatcherPriority.Background);
        }

        static void RaiseRequerySuggestedImpl()
        {
            var current = Current;

            // Call the RequerySuggested handlers
            current.PrivateRequerySuggested?.Invoke(null, EventArgs.Empty);

            //event handlers done, so we could enable fire again.
            current._requerySuggestedOperation = null;
        }
    }

    private sealed class WeakHandlerWrapper : IWeakEventSubscriber<EventArgs>
    {
        private readonly EventHandler _handler;

        public WeakHandlerWrapper(EventHandler handler)
        {
            _handler = handler;
        }

        public void OnEvent(object sender, WeakEvent ev, EventArgs e)
        {
            _handler(sender, e);
        }

        public override int GetHashCode()
        {
            return _handler.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is WeakHandlerWrapper other && other._handler == _handler;
        }
    }
}