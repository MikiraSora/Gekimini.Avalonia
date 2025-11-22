using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;
using Gekimini.Avalonia.Modules.MainMenu.Views.MenuItems;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.MainMenu.Views;

public partial class MenuItemDataTemplate : IDataTemplate
{
    [GetServiceLazy]
    public partial ViewLocator ViewLocator { get; }

    public Control Build(object param)
    {
        if (param is SeparatorItemViewModel)
            return new SeparatorItemView {DataContext = param};
        return ViewLocator.Build(param);
    }

    public bool Match(object data)
    {
        return data is MenuItemViewModelBase;
    }
}