using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;

public class FluentColorTheme : IColorTheme
{
    private readonly ThemeVariant overrideVariant;
    private FluentTheme cachedNewFluent;

    private FluentTheme prevFluent;
    private ThemeVariant prevVariant;

    public FluentColorTheme(string name, ThemeVariant overrideVariant, Uri palleteUri)
    {
        this.overrideVariant = overrideVariant;
        Name = name;

        /*
        resource = AvaloniaXamlLoader.Load(palleteUri) as ColorPaletteResources;
        if (resource is null)
            throw new Exception("无法加载 Fluent ColorPaletteResources: " + palleteUri);
        */
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

    private string GetKeyName(string key)
    {
        if (string.IsNullOrEmpty(key))
            return "_themeManager_";

        return "_themeManager_" + char.ToLowerInvariant(key[0]) + key.Substring(1);
    }

    private FluentTheme GetOrCreateFluent(Application app)
    {
        if (cachedNewFluent is not null)
            return cachedNewFluent;

        cachedNewFluent = new FluentTheme();

        var key = GetKeyName(Name);

        try
        {
            var resource = app.FindResource(key) as ColorPaletteResources;
            cachedNewFluent.Palettes[overrideVariant] = resource;
        }
        catch (Exception e)
        {
            //todo
        }

        return cachedNewFluent;
    }
}