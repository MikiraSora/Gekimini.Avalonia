using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Models.Settings;

public partial class GekiminiSetting : ObservableObject
{
    public static JsonTypeInfo<GekiminiSetting> JsonTypeInfo => JsonSourceGenerateContext.Default.GekiminiSetting;

    [ObservableProperty]
    public partial bool AutoHideMainMenu { get; set; } = false;

    [ObservableProperty]
    public partial string ThemeName { get; set; } = "LightTheme";

    [ObservableProperty]
    public partial string LanguageCode { get; set; } = string.Empty;

    [ObservableProperty]
    public partial double MainWindowRectLeft { get; set; } = 460;

    [ObservableProperty]
    public partial double MainWindowRectTop { get; set; } = 190;

    [ObservableProperty]
    public partial double MainWindowRectWidth { get; set; } = 1000;

    [ObservableProperty]
    public partial double MainWindowRectHeight { get; set; } = 800;

    [ObservableProperty]
    public partial string ShellLayout { get; set; }
}