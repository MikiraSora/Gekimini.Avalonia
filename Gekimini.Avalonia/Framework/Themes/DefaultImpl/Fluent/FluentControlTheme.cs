using System;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;

public class FluentControlTheme : IControlTheme
{
    private readonly FluentTheme fluentTheme;

    public FluentControlTheme()
    {
        fluentTheme =
            AvaloniaXamlLoader.Load(
                    new Uri("avares://Gekimini.Avalonia/Framework/Themes/DefaultImpl/Fluent/Styles/FluentTheme.axaml"))
                as
                FluentTheme;
    }

    public string Name => "Fluent";

    public void ApplyControlTheme()
    {
        var app = Application.Current!;
        if (app is null)
            return;

        if (app.Styles.Any(x => x is FluentTheme))
            return;
        app.Styles.Insert(0, fluentTheme);
    }

    public void RevertControlTheme()
    {
        var app = Application.Current!;
        if (app is null)
            return;

        app.Styles.Remove(fluentTheme);
    }
}