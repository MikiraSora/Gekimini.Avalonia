using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Gekimini.Avalonia.Models.Events;

namespace Gekimini.Avalonia.Framework.Languages;

public abstract class LocalizedString : INotifyPropertyChanged, IRecipient<CurrentCultureInfoChangedEvent>
{
    private static readonly Dictionary<string, WeakReference<LocalizedString>> cachedResourceKeyToLocalizedStringMap =
        new();

    protected LocalizedString(bool shouldRegisterEvent)
    {
        if (shouldRegisterEvent)
            WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public string Text => GetText();

    public event PropertyChangedEventHandler PropertyChanged;

    public void Receive(CurrentCultureInfoChangedEvent message)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
    }

    protected abstract string GetText();

    public static LocalizedString CreateFromResourceKey(string resourceKey)
    {
        if (cachedResourceKeyToLocalizedStringMap.TryGetValue(resourceKey, out var cacheStringReference))
            if (cacheStringReference.TryGetTarget(out var localizedString))
                return localizedString;

        var newObj = new ResourceLocalizedString(resourceKey);
        cachedResourceKeyToLocalizedStringMap[resourceKey] = new WeakReference<LocalizedString>(newObj);
        return newObj;
    }

    public static LocalizedString CreateFromRawText(string rawText)
    {
        return new RawTextString(rawText);
    }

    public static LocalizedString CreateFromTemplateFunc(Func<string> templateFunc)
    {
        return new TemplateLocalizedString(templateFunc);
    }
}