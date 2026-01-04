using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl;

[RegisterSingleton<IThemeManager>]
public partial class DefaultThemeManager : ObservableObject, IThemeManager
{
    private readonly ISettingManager settingManager;
    private bool initialized;

    public DefaultThemeManager(IEnumerable<IColorTheme> colorThemes, IEnumerable<IControlTheme> controlThemes,
        ISettingManager settingManager)
    {
        this.settingManager = settingManager;
        ColorThemes = colorThemes.ToList();
        ControlThemes = controlThemes.ToList();
    }

    private List<IColorTheme> ColorThemes { get; }
    private List<IControlTheme> ControlThemes { get; }

    [GetServiceLazy]
    public partial ILogger<DefaultThemeManager> Logger { get; }

    [ObservableProperty]
    public partial IColorTheme CurrentColorTheme { get; set; }

    [ObservableProperty]
    public partial IControlTheme CurrentControlTheme { get; set; }

    public IEnumerable<IColorTheme> AvaliableColorThemes => ColorThemes;
    public IEnumerable<IControlTheme> AvaliableControlThemes => ControlThemes;

    public void Initalize()
    {
        if (initialized)
            return;

        initialized = true;

        var setting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        var pickControlTheme = AvaliableControlThemes.FirstOrDefault(x =>
            x.Name.Equals(setting.ControlThemeName, StringComparison.OrdinalIgnoreCase));
        var pickColorTheme = AvaliableColorThemes.FirstOrDefault(x =>
            x.Name.Equals(setting.ColorThemeName, StringComparison.OrdinalIgnoreCase));

        CurrentControlTheme = pickControlTheme ?? AvaliableControlThemes.FirstOrDefault();
        CurrentColorTheme = pickColorTheme ?? AvaliableColorThemes.LastOrDefault();
    }

    partial void OnCurrentColorThemeChanged(IColorTheme oldValue, IColorTheme newValue)
    {
        if (newValue == null) return;
        ApplyColorTheme(oldValue, newValue);
    }

    partial void OnCurrentControlThemeChanged(IControlTheme oldValue, IControlTheme newValue)
    {
        if (newValue == null) return;
        ApplyControlTheme(oldValue, newValue);
    }

    private void ApplyColorTheme(IColorTheme oldValue, IColorTheme colorTheme)
    {
        oldValue?.RevertColorTheme();
        colorTheme?.ApplyColorTheme();

        Logger.LogInformationEx($"Applying color theme: {colorTheme?.Name ?? "<null>"}");
    }

    private void ApplyControlTheme(IControlTheme oldValue, IControlTheme controlTheme)
    {
        oldValue?.RevertControlTheme();
        controlTheme?.ApplyControlTheme();

        Logger.LogInformationEx($"Applying control theme: {controlTheme?.Name ?? "<null>"}");
    }
}