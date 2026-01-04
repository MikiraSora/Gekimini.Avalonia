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

    public MainMenuViewModel(IMenuBuilder menuBuilder, ISettingManager settingManager)
    {
        var gekiminiSetting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        _autoHide = gekiminiSetting.AutoHideMainMenu;

        menuBuilder.BuildMenuBar(MenuDefinitions.MainMenuBar, this);
    }

    //todo not support
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
}