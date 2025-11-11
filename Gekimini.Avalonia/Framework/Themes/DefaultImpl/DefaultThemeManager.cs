using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

[RegisterSingleton<IThemeManager>]
public partial class DefaultThemeManager : ObservableObject, IThemeManager
{
    private readonly ResourceDictionary applicationResources = new();
    private bool initialized;

    private List<ColorTheme> ColorThemes { get; } = new();
    private List<ControlTheme> ControlThemes { get; } = new();

    [ObservableProperty]
    public partial ColorTheme CurrentColorTheme { get; set; }

    [ObservableProperty]
    public partial ControlTheme CurrentControlTheme { get; set; }

    public void InitalizeThemes()
    {
        if (initialized)
            return;

        initialized = true;

        CurrentColorTheme = AvaliableColorThemes.FirstOrDefault();
        CurrentControlTheme = AvaliableControlThemes.FirstOrDefault();

        App.Current.Resources.MergedDictionaries.Add(applicationResources);
    }

    public IEnumerable<ColorTheme> AvaliableColorThemes => ColorThemes;
    public IEnumerable<ControlTheme> AvaliableControlThemes => ControlThemes;

    partial void OnCurrentColorThemeChanged(ColorTheme oldValue, ColorTheme newValue)
    {
        if (newValue == null) return;
        ApplyColorTheme(newValue);
    }

    partial void OnCurrentControlThemeChanged(ControlTheme oldValue, ControlTheme newValue)
    {
        if (newValue == null) return;
        ApplyControlTheme(newValue);
    }

    private void ApplyColorTheme(ColorTheme theme)
    {
        //todo
    }

    private void ApplyControlTheme(ControlTheme theme)
    {
        //todo
    }
}