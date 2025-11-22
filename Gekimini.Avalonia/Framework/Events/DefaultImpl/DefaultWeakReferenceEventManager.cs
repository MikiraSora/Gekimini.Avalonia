using System;
using System.Collections.Generic;
using System.Linq;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Events.DefaultImpl;

[RegisterSingleton<IWeakReferenceEventManager>]
public class DefaultWeakReferenceEventManager : IWeakReferenceEventManager
{
    private readonly Dictionary<Type, List<HandlerEntry>> _handlers = new();

    //[GetServiceLazy]
    //private partial ILogger<DefaultWeakReferenceEventManager> Logger { get; }

    public IDisposable RegisterEvent<MESSAGE>(IWeakReferenceEventManager.EventHandler<MESSAGE> eventHandler)
        where MESSAGE : IMessage
    {
        var type = typeof(MESSAGE);
        var entry = new HandlerEntry<MESSAGE>(eventHandler);

        var list = GetHandlerList(type);
        if (list.FirstOrDefault(h => h.Id == entry.Id) is { } existEntry)
            list.Remove(existEntry);

        list.Add(entry);

        return new UnregisterDisposable<MESSAGE>(this, eventHandler);
    }

    public IDisposable RegisterEvent<MESSAGE, RESPONSE>(
        IWeakReferenceEventManager.ResponseEventHandler<MESSAGE, RESPONSE> eventHandler)
        where MESSAGE : IResponsibleMessage<RESPONSE>
    {
        var type = typeof(MESSAGE);
        var entry = new HandlerEntry<MESSAGE, RESPONSE>(eventHandler);

        var list = GetHandlerList(type);
        if (list.FirstOrDefault(h => h.Id == entry.Id) is { } existEntry)
            list.Remove(existEntry);

        list.Add(entry);

        return new UnregisterDisposable<MESSAGE, RESPONSE>(this, eventHandler);
    }

    public void UnregisterEvent<MESSAGE>(IWeakReferenceEventManager.EventHandler<MESSAGE> eventHandler)
        where MESSAGE : IMessage
    {
        Remove(eventHandler);
    }

    public void UnregisterEvent<MESSAGE, RESPONSE>(
        IWeakReferenceEventManager.ResponseEventHandler<MESSAGE, RESPONSE> eventHandler)
        where MESSAGE : IResponsibleMessage<RESPONSE>
    {
        Remove(eventHandler);
    }

    public void SendMessage<MESSAGE>(MESSAGE message) where MESSAGE : IMessage
    {
        var type = typeof(MESSAGE);
        var list = GetHandlerList(type);

        var needRemoveCheck = false;
        foreach (var entry in list.OfType<HandlerEntry<MESSAGE>>())
            needRemoveCheck |= !entry.TryInvoke(message);

        if (needRemoveCheck)
            list.RemoveAll(h => !h.IsAlive);
    }

    public IEnumerable<RESPONSE> SendMessageAndGetResponses<MESSAGE, RESPONSE>(MESSAGE message)
        where MESSAGE : IResponsibleMessage<RESPONSE>
    {
        var type = typeof(MESSAGE);
        var list = GetHandlerList(type);

        var needRemoveCheck = false;

        foreach (var entry in list.OfType<HandlerEntry<MESSAGE, RESPONSE>>())
        {
            var isInvokeSuccess = entry.TryInvoke(message, out var result);
            if (isInvokeSuccess)
                yield return result;
            else
                needRemoveCheck = true;
        }

        if (needRemoveCheck)
            list.RemoveAll(h => !h.IsAlive);
    }

    private List<HandlerEntry> GetHandlerList(Type type)
    {
        if (_handlers.TryGetValue(type, out var list))
            return list;
        return _handlers[type] = [];
    }

    private void Remove<MESSAGE>(IWeakReferenceEventManager.EventHandler<MESSAGE> handler) where MESSAGE : IMessage
    {
        var type = typeof(MESSAGE);
        var list = GetHandlerList(type);

        list.RemoveAll(h => h.MatchDelegate(handler) || !h.IsAlive);
    }

    private void Remove<MESSAGE, RESPONSE>(IWeakReferenceEventManager.ResponseEventHandler<MESSAGE, RESPONSE> handler)
        where MESSAGE : IResponsibleMessage<RESPONSE>
    {
        var type = typeof(MESSAGE);
        var list = GetHandlerList(type);

        list.RemoveAll(h => h.MatchDelegate(handler) || !h.IsAlive);
    }

    private abstract class HandlerEntry
    {
        public abstract bool IsAlive { get; }
        public abstract int Id { get; }
        public abstract bool MatchDelegate(Delegate d);
    }

    private class HandlerEntry<TMessage> : HandlerEntry where TMessage : IMessage
    {
        private readonly WeakReference<IWeakReferenceEventManager.EventHandler<TMessage>> HandlerRef;

        private bool isAlive = true;

        public HandlerEntry(IWeakReferenceEventManager.EventHandler<TMessage> handler)
        {
            Id = handler.GetHashCode();
            HandlerRef = new WeakReference<IWeakReferenceEventManager.EventHandler<TMessage>>(handler);
        }

        public override int Id { get; }

        public override bool IsAlive => isAlive && (isAlive = HandlerRef.TryGetTarget(out _));

        public bool TryInvoke(TMessage message)
        {
            if (HandlerRef.TryGetTarget(out var handler))
            {
                handler(message);
                return true;
            }

            isAlive = false;
            return false;
        }

        public override bool MatchDelegate(Delegate d)
        {
            return HandlerRef.TryGetTarget(out var target)
                   && target == (IWeakReferenceEventManager.EventHandler<TMessage>) d;
        }
    }

    private class HandlerEntry<TMessage, TResponse> : HandlerEntry where TMessage : IResponsibleMessage<TResponse>
    {
        private readonly WeakReference<IWeakReferenceEventManager.ResponseEventHandler<TMessage, TResponse>> HandlerRef;
        private readonly string name;
        private bool isAlive;

        public HandlerEntry(IWeakReferenceEventManager.ResponseEventHandler<TMessage, TResponse> handler)
        {
            Id = handler.GetHashCode();
            name = $"target:{handler.Target?.GetType().Name} method:{handler.Method.Name}";

            HandlerRef =
                new WeakReference<IWeakReferenceEventManager.ResponseEventHandler<TMessage, TResponse>>(handler);
        }

        public override int Id { get; }

        public override bool IsAlive => isAlive && (isAlive = HandlerRef.TryGetTarget(out _));

        public override string ToString()
        {
            return $"alive:{IsAlive} id:{Id} name:{name}";
        }

        public bool TryInvoke(TMessage message, out TResponse result)
        {
            if (HandlerRef.TryGetTarget(out var handler))
            {
                result = handler(message);
                return true;
            }

            result = default;
            isAlive = false;
            return false;
        }

        public override bool MatchDelegate(Delegate d)
        {
            return HandlerRef.TryGetTarget(out var target)
                   && target == (IWeakReferenceEventManager.ResponseEventHandler<TMessage, TResponse>) d;
        }
    }

    private class UnregisterDisposable<MESSAGE> : IDisposable where MESSAGE : IMessage
    {
        private readonly IWeakReferenceEventManager.EventHandler<MESSAGE> _handler;
        private readonly DefaultWeakReferenceEventManager _mgr;

        public UnregisterDisposable(
            DefaultWeakReferenceEventManager mgr, IWeakReferenceEventManager.EventHandler<MESSAGE> handler)
        {
            _mgr = mgr;
            _handler = handler;
        }

        public void Dispose()
        {
            _mgr.Remove(_handler);
        }
    }

    private class UnregisterDisposable<MESSAGE, RESPONSE> : IDisposable where MESSAGE : IResponsibleMessage<RESPONSE>
    {
        private readonly IWeakReferenceEventManager.ResponseEventHandler<MESSAGE, RESPONSE> _handler;
        private readonly DefaultWeakReferenceEventManager _mgr;

        public UnregisterDisposable(
            DefaultWeakReferenceEventManager mgr,
            IWeakReferenceEventManager.ResponseEventHandler<MESSAGE, RESPONSE> handler)
        {
            _mgr = mgr;
            _handler = handler;
        }

        public void Dispose()
        {
            _mgr.Remove(_handler);
        }
    }
}