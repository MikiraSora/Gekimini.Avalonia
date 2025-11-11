using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

public static class DefaultColorThemes
{
    [RegisterStaticObject]
    public static ColorTheme Dark = new()
    {
        Name = "Dark"
    };

    [RegisterStaticObject]
    public static ColorTheme Light = new()
    {
        Name = "Light"
    };
}