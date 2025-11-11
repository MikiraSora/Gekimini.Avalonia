using System.Collections.ObjectModel;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;

public class MenuItemViewModelBase : ViewModelBase
{
    public static MenuItemViewModelBase Separator => new SeparatorItemViewModel();

    public ObservableCollection<MenuItemViewModelBase> Children { get; } = new();

    public void Add(params MenuItemViewModelBase[] menuItems)
    {
        foreach (var menuItem in menuItems)
            Children.Add(menuItem);
    }
}