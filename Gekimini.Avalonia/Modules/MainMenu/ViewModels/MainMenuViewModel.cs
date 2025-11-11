using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels;

[RegisterSingleton<IMenu>]
public class MainMenuViewModel : ViewModelBase, IMenu
{
    private readonly IMenuBuilder _menuBuilder;

    private bool _autoHide;

    public MainMenuViewModel(IMenuBuilder menuBuilder, ISettingManager settingManager)
    {
        _menuBuilder = menuBuilder;
        var gekiminiSetting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        _autoHide = gekiminiSetting.AutoHideMainMenu;

        _menuBuilder.BuildMenuBar(MenuDefinitions.MainMenuBar, this);
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

    public ObservableCollection<MenuItemViewModelBase> MenuItems { get; set; } = new();

    public void OnViewAfterLoaded(Control view)
    {
        ViewAfterLoaded?.Invoke(view);
    }

    public void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
}