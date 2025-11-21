using Avalonia;
using Avalonia.Styling;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

public class BuiltInFluentColorTheme : IColorTheme
{
    private readonly ThemeVariant themeVariant;
    private ThemeVariant prevVariant;

    public BuiltInFluentColorTheme(string name, ThemeVariant themeVariant)
    {
        Name = name;
        this.themeVariant = themeVariant;
    }

    public string Name { get; }

    public void ApplyColorTheme()
    {
        prevVariant = Application.Current?.RequestedThemeVariant;
        Application.Current?.RequestedThemeVariant = themeVariant;
    }

    public void RevertColorTheme()
    {
        Application.Current?.RequestedThemeVariant = prevVariant;
    }
}