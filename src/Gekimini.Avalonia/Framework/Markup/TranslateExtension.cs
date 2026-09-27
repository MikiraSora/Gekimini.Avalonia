using System;
using Avalonia;
using Avalonia.Data;
using Microsoft.Extensions.Logging;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Framework.Markup;

public class TranslateExtension
{
    private static readonly CompiledBindingPath textPath =
        CompiledBinding.Create<ILocalizedTextSource, string>(source => source.Text).Path!;

    private readonly ILocalizedTextSource textSource;

    public TranslateExtension(ILocalizedTextSource textSource)
    {
        this.textSource = textSource;
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (textSource == null)
            return "<i18n:null-text-source>";

        return new CompiledBinding(textPath)
        {
            Source = textSource,
            Mode = BindingMode.OneWay
        };
    }
}