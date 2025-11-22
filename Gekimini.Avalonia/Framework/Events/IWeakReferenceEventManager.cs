using System;
using System.Collections.Generic;

namespace Gekimini.Avalonia.Framework.Events;

public interface IWeakReferenceEventManager
{
    public delegate void EventHandler<in MESSAGE>(MESSAGE message) where MESSAGE : IMessage;

    public delegate RESPONSE ResponseEventHandler<in MESSAGE, out RESPONSE>(MESSAGE message)
        where MESSAGE : IResponsibleMessage<RESPONSE>;

    IDisposable RegisterEvent<MESSAGE, RESPONSE>(ResponseEventHandler<MESSAGE, RESPONSE> eventHandler)
        where MESSAGE : IResponsibleMessage<RESPONSE>;

    IDisposable RegisterEvent<MESSAGE>(EventHandler<MESSAGE> eventHandler)
        where MESSAGE : IMessage;
    
    void UnregisterEvent<MESSAGE, RESPONSE>(ResponseEventHandler<MESSAGE, RESPONSE> eventHandler)
        where MESSAGE : IResponsibleMessage<RESPONSE>;

    void UnregisterEvent<MESSAGE>(EventHandler<MESSAGE> eventHandler)
        where MESSAGE : IMessage;

    void SendMessage<T>(T message) where T : IMessage;
    IEnumerable<X> SendMessageAndGetResponses<T, X>(T message) where T : IResponsibleMessage<X>;
    IEnumerable<X> SendMessageAndGetResponses<X>(IResponsibleMessage<X> message)
    {
        return SendMessageAndGetResponses<IResponsibleMessage<X>, X>(message);
    }
}