using System;
using Avalonia.Reactive;

namespace Gekimini.Avalonia;

public static class IObservableEx
{
    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext,
        Action<Exception>? onError = default,
        Action onCompleted = default)
    {
        return source.Subscribe(new AnonymousObserver<T>(onNext, onError ?? (e => { })
            , onCompleted ?? (() => { })));
    }
}