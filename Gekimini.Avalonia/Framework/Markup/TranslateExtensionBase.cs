using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Languages.DefaultImpl;
using Gekimini.Avalonia.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Framework.Markup;

public class TranslateExtensionBase
{
    private readonly string _resKey;
    private readonly INotifyPropertyChanged source;

    public TranslateExtensionBase(string resKey, Func<string, CultureInfo, string> callback)
    {
        _resKey = resKey;
        var language = default(ILanguageManager);
#if DEBUG
        if (DesignModeHelper.IsDesignMode)
            language = new DefaultLanguageManager(default);
#endif
        language = language ?? (App.Current as App).ServiceProvider.GetService<ILanguageManager>();
        source = language.GetTranslationSource(callback);
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(_resKey))
            return string.Empty;

        var binding = new Binding(_resKey)
        {
            Source = source,
            Mode = BindingMode.OneWay
        };

        return binding;
    }
}