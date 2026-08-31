using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Themes;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels;

[RegisterSingleton<ISettingsEditor>]
public partial class MainMenuSettingsViewModel : ViewModelBase, ISettingsEditor
{
    private readonly ILanguageManager _languageManager;
    private readonly ISettingManager _settingManager;
    private readonly IThemeManager _themeManager;
    private readonly GekiminiSetting settings;

    public MainMenuSettingsViewModel(IThemeManager themeManager, ILanguageManager languageManager,
        ISettingManager settingManager)
    {
        _themeManager = themeManager;
        _languageManager = languageManager;
        _settingManager = settingManager;

        settings = _settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        AutoHideMainMenu = settings.AutoHideMainMenu;
        
        SelectedColorTheme = _themeManager.CurrentColorTheme;
        SelectedControlTheme = _themeManager.CurrentControlTheme;

        SelectedLanguage = _languageManager.GetCurrentLanguage();
    }

    public IEnumerable<string> Languages => _languageManager.GetAvaliableLanguageNames();

    [ObservableProperty]
    public partial string SelectedLanguage { get; set; }

    [ObservableProperty]
    public partial bool AutoHideMainMenu { get; set; }

    public IEnumerable<IControlTheme> ControlThemes => _themeManager.AvaliableControlThemes;
    public IEnumerable<IColorTheme> ColorThemes => _themeManager.AvaliableColorThemes;

    [ObservableProperty]
    public partial IColorTheme SelectedColorTheme { get; set; }

    [ObservableProperty]
    public partial IControlTheme SelectedControlTheme { get; set; }

    public string SettingsPageName => ProgramLanguages.SettingsPageGeneral;

    public string SettingsPagePath => ProgramLanguages.SettingsPathEnvironment;

    public void ApplyChanges()
    {
        if (SelectedColorTheme is not null)
            _themeManager.CurrentColorTheme = SelectedColorTheme;
        if (SelectedControlTheme is not null)
            _themeManager.CurrentControlTheme = SelectedControlTheme;

        _languageManager.SetLanguage(SelectedLanguage);

        settings.ColorThemeName = _themeManager.CurrentColorTheme?.Name ?? "Light";
        settings.ControlThemeName = _themeManager.CurrentControlTheme?.Name ?? "Fluent";
        settings.AutoHideMainMenu = AutoHideMainMenu;
        settings.LanguageCode = _languageManager.GetCurrentLanguage();

        _settingManager.SaveSetting(settings, GekiminiSetting.JsonTypeInfo);
    }

    public void ResetDefault()
    {
        AutoHideMainMenu = false;
        SelectedColorTheme = FindColorTheme(_themeManager.AvaliableColorThemes, "Light")
                             ?? _themeManager.CurrentColorTheme
                             ?? _themeManager.AvaliableColorThemes?.FirstOrDefault();
        SelectedControlTheme = FindControlTheme(_themeManager.AvaliableControlThemes, "Fluent")
                               ?? _themeManager.CurrentControlTheme
                               ?? _themeManager.AvaliableControlThemes?.FirstOrDefault();

        var availableLanguages = Languages?.ToArray() ?? [];
        SelectedLanguage = availableLanguages.FirstOrDefault(
                               language => language.Equals("Default", StringComparison.OrdinalIgnoreCase))
                           ?? "Default";

        // Reset is an explicit user action, so persist and apply it immediately.
        ApplyChanges();
    }

    private static IColorTheme FindColorTheme(IEnumerable<IColorTheme> themes, string name) =>
        themes?.FirstOrDefault(theme =>
            theme?.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

    private static IControlTheme FindControlTheme(IEnumerable<IControlTheme> themes, string name) =>
        themes?.FirstOrDefault(theme =>
            theme?.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
}
