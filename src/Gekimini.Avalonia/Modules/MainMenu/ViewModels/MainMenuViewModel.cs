using System.Collections.ObjectModel;
using System.ComponentModel;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels;

[RegisterSingleton<IMenu>]
public class MainMenuViewModel : ViewModelBase, IMenu
{
    private bool _autoHide;
    private readonly GekiminiSetting gekiminiSetting;

    public MainMenuViewModel(IMenuBuilder menuBuilder, ISettingManager settingManager)
    {
        gekiminiSetting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        _autoHide = gekiminiSetting.AutoHideMainMenu;
        gekiminiSetting.PropertyChanged += OnSettingPropertyChanged;

        menuBuilder.BuildMenuBar(MenuDefinitions.MainMenuBar, this);
    }

    public bool AutoHide
    {
        get => _autoHide;
        private set
        {
            if (_autoHide == value)
                return;

            _autoHide = value;

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AutoHide)));
        }
    }

    public ObservableCollection<MenuItemViewModelBase> MenuItems { get; set; } = [];

    private void OnSettingPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GekiminiSetting.AutoHideMainMenu))
            AutoHide = gekiminiSetting.AutoHideMainMenu;
    }
}
