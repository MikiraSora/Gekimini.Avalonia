using System.Collections.Generic;
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
public class MainMenuSettingsViewModel : ViewModelBase, ISettingsEditor
{
    private readonly List<string> _availableLanguages = new();
    private readonly ILanguageManager _languageManager;
    private readonly ISettingManager _settingManager;
    private readonly IThemeManager _themeManager;
    private readonly GekiminiSetting settings;
    private bool _autoHideMainMenu;
    private string _selectedLanguage;

    public MainMenuSettingsViewModel(IThemeManager themeManager, ILanguageManager languageManager,
        ISettingManager settingManager)
    {
        _themeManager = themeManager;
        _languageManager = languageManager;
        _settingManager = settingManager;

        settings = _settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        AutoHideMainMenu = settings.AutoHideMainMenu;

        SelectedLanguage = _languageManager.GetCurrentLanguage();
        _availableLanguages.AddRange(_languageManager.GetAvaliableLanguageNames());
    }

    public IEnumerable<string> Languages => _availableLanguages;

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (value.Equals(_selectedLanguage))
                return;
            _selectedLanguage = value;
            OnPropertyChanged();
        }
    }

    public bool AutoHideMainMenu
    {
        get => _autoHideMainMenu;
        set
        {
            if (value.Equals(_autoHideMainMenu))
                return;
            _autoHideMainMenu = value;
            OnPropertyChanged();
        }
    }

    public string SettingsPageName => Resources.SettingsPageGeneral;

    public string SettingsPagePath => Resources.SettingsPathEnvironment;

    public void ApplyChanges()
    {
        settings.ThemeName = _themeManager.CurrentColorTheme.Name;
        settings.AutoHideMainMenu = AutoHideMainMenu;
        _settingManager.SaveSetting(settings, GekiminiSetting.JsonTypeInfo);

        _languageManager.SetLanguage(SelectedLanguage);
    }
}