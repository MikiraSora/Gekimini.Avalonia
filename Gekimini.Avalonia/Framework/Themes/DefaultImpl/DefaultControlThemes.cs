using Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

public static class DefaultControlThemes
{
    [RegisterStaticObject]
    public static readonly IControlTheme Fluent = new FluentControlTheme();
}