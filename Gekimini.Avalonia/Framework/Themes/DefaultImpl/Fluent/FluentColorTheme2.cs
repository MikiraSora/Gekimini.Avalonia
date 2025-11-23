using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;

public class FluentColorTheme2<T> : IColorTheme where T : ColorPaletteResources, new()
{
    private readonly ThemeVariant overrideVariant;
    private FluentTheme cachedNewFluent;

    private FluentTheme prevFluent;
    private ThemeVariant prevVariant;

    public FluentColorTheme2(string name, ThemeVariant overrideVariant)
    {
        this.overrideVariant = overrideVariant;
        Name = name;
    }

    public string Name { get; }

    public void ApplyColorTheme()
    {
        var app = Application.Current;
        if (app is null)
            return;

        prevFluent = app.Styles.OfType<FluentTheme>().FirstOrDefault();
        var newFluent = GetOrCreateFluent(app);

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

        var newFluent = GetOrCreateFluent(app);

        app.Styles.Remove(newFluent);
        app.Styles.Insert(0, prevFluent);

        prevFluent = default;
        app.RequestedThemeVariant = prevVariant;
        prevVariant = default;
    }

    private FluentTheme GetOrCreateFluent(Application app)
    {
        if (cachedNewFluent is not null)
            return cachedNewFluent;

        cachedNewFluent = new FluentTheme();
        cachedNewFluent.Palettes[overrideVariant] = new T();

        return cachedNewFluent;
    }
}