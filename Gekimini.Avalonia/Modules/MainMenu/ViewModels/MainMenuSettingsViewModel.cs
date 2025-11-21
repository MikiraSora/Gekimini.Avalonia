using System.Collections.Generic;
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

[RegisterTransient<ISettingsEditor>]
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

    public string SettingsPageName => Resources.SettingsPageGeneral;

    public string SettingsPagePath => Resources.SettingsPathEnvironment;

    public void ApplyChanges()
    {
        _themeManager.CurrentColorTheme = SelectedColorTheme;
        _themeManager.CurrentControlTheme = SelectedControlTheme;

        _languageManager.SetLanguage(SelectedLanguage);

        settings.ColorThemeName = _themeManager.CurrentColorTheme.Name;
        settings.ControlThemeName = _themeManager.CurrentControlTheme.Name;
        settings.AutoHideMainMenu = AutoHideMainMenu;
        settings.LanguageCode = _languageManager.GetCurrentLanguage();

        _settingManager.SaveSetting(settings, GekiminiSetting.JsonTypeInfo);
    }
}