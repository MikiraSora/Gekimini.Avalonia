using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.Framework.Languages;

public class TranslationSource : ObservableObject
{
    private readonly Func<string, string> getStringFunc;

    public TranslationSource(Func<string, string> getStringFunc)
    {
        this.getStringFunc = getStringFunc;
    }

    public string this[string key] => getStringFunc(key);

    public void Refresh()
    {
        OnPropertyChanged("");
    }
}