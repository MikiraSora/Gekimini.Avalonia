using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Gekimini.Avalonia.Framework.Languages;

public interface ILanguageManager
{
    public IEnumerable<string> GetAvaliableLanguageNames();
    public void SetLanguage(string languageName);
    public string GetCurrentLanguage();
    INotifyPropertyChanged GetTranslationSource(Func<string, CultureInfo, string> callback);
    INotifyPropertyChanged GetTranslationSource(string resKey);
}