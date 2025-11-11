using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia.Controls;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.MainMenu;

public interface IMenu : IViewModel
{
    ObservableCollection<MenuItemViewModelBase> MenuItems { get; }
}