using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes;

public interface IColorTheme
{
    string Name { get; }

    void ApplyColorTheme();
    void RevertColorTheme();
}