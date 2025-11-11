using Avalonia.Styling;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

public static class DefaultControlThemes
{
    [RegisterStaticObject]
    public static ControlTheme Default = new()
    {
        Styles = new Styles(),
        Name = "Default"
    };
}