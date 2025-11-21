using System;
using Avalonia.Styling;
using Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

public static class DefaultColorThemes
{
    [RegisterStaticObject]
    public static readonly IColorTheme Default = new BuiltInFluentColorTheme("Default", ThemeVariant.Default);
    /*
    [RegisterStaticObject]
    public static readonly IColorTheme Dark = new BuiltInFluentColorTheme("Dark", ThemeVariant.Dark);

    [RegisterStaticObject]
    public static readonly IColorTheme Light = new BuiltInFluentColorTheme("Light", ThemeVariant.Light);
    */
    [RegisterStaticObject]
    public static readonly IColorTheme Dark = new FluentColorTheme("Dark", ThemeVariant.Dark,
        new Uri("avares://Gekimini.Avalonia/Framework/Themes/DefaultImpl/Fluent/Palletes/Dark.axaml"));

    [RegisterStaticObject]
    public static readonly IColorTheme Light = new FluentColorTheme("Light", ThemeVariant.Light,
        new Uri("avares://Gekimini.Avalonia/Framework/Themes/DefaultImpl/Fluent/Palletes/Light.axaml"));
    
    [RegisterStaticObject]
    public static readonly IColorTheme LavenderDark = new FluentColorTheme("LavenderDark", ThemeVariant.Dark,
        new Uri("avares://Gekimini.Avalonia/Framework/Themes/DefaultImpl/Fluent/Palletes/LavenderDark.axaml"));

    [RegisterStaticObject]
    public static readonly IColorTheme LavenderLight = new FluentColorTheme("LavenderLight", ThemeVariant.Light,
        new Uri("avares://Gekimini.Avalonia/Framework/Themes/DefaultImpl/Fluent/Palletes/LavenderLight.axaml"));
}