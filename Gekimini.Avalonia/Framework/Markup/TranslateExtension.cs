using System;
using Avalonia;
using Avalonia.Data;
using Microsoft.Extensions.Logging;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Framework.Markup;

public class TranslateExtension
{
    private readonly ILocalizedTextSource textSource;

    public TranslateExtension(ILocalizedTextSource textSource)
    {
        this.textSource = textSource;
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (textSource == null)
            return "<i18n:null-text-source>";

        var binding = new Binding(nameof(ILocalizedTextSource.Text))
        {
            Source = textSource,
            Mode = BindingMode.OneWay
        };

        return binding;
    }
}