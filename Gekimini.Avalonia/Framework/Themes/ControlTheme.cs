using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Gekimini.Avalonia.Framework.Themes;

public class ControlTheme
{
    public required Styles Styles { get; init; }

    public required string Name { get; init; }

    public static ControlTheme LoadFromXaml(Uri uri, string name)
    {
        var styles = (Styles) AvaloniaXamlLoader.Load(uri)!;
        return new ControlTheme
        {
            Styles = styles, Name = name
        };
    }
}