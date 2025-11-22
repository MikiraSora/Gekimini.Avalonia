using System;
using Avalonia.Controls;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;

namespace Gekimini.Avalonia.Modules.MainMenu.Controls;

public class MenuItemEx : MenuItem
{
    protected override Type StyleKeyOverride => typeof(MenuItem);

    protected override Control CreateContainerForItemOverride(object item, int index, object recycleKey)
    {
        if (item is SeparatorItemViewModel)
            return new Separator();

        return new MenuItemEx();
    }
}