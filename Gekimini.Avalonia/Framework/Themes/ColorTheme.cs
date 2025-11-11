using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Gekimini.Avalonia.Framework.Themes;

public class ColorTheme
{
    public ResourceDictionary ThemeDictionary { get; set; }

    public required string Name { get; init; }

    public static ColorTheme LoadFromXaml(Uri uri, string name)
    {
        var dictionary = (ResourceDictionary) AvaloniaXamlLoader.Load(uri)!;
        return new ColorTheme
        {
            ThemeDictionary = dictionary, Name = name
        };
    }
}