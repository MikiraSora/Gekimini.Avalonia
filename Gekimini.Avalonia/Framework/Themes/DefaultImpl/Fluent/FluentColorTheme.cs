using System;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;

public class FluentColorTheme : IColorTheme
{
    private readonly FluentTheme newFluent;
    private readonly ThemeVariant overrideVariant;
    private readonly ColorPaletteResources resource;

    private FluentTheme prevFluent;
    private ThemeVariant prevVariant;

    public FluentColorTheme(string name, ThemeVariant overrideVariant, Uri palleteUri)
    {
        this.overrideVariant = overrideVariant;
        Name = name;
        resource = AvaloniaXamlLoader.Load(palleteUri) as ColorPaletteResources;
        if (resource is null)
            throw new Exception("无法加载 Fluent ColorPaletteResources: " + palleteUri);

        newFluent = new FluentTheme();
        newFluent.Palettes[overrideVariant] = resource;
    }

    public string Name { get; }

    public void ApplyColorTheme()
    {
        var app = Application.Current;
        if (app is null)
            return;

        prevFluent = app.Styles.OfType<FluentTheme>().FirstOrDefault();

        if (prevFluent is not null)
            app.Styles.Remove(prevFluent);
        app.Styles.Insert(0, newFluent);

        prevVariant = app.RequestedThemeVariant;
        app.RequestedThemeVariant = overrideVariant;
    }

    public void RevertColorTheme()
    {
        var app = Application.Current;
        if (app is null)
            return;

        if (newFluent is not null)
            app.Styles.Remove(newFluent);
        app.Styles.Insert(0, prevFluent);

        prevFluent = default;
        app.RequestedThemeVariant = prevVariant;
        prevVariant = default;
    }
}