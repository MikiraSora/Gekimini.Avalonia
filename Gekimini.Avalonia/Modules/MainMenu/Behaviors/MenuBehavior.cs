using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Gekimini.Avalonia.Framework.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.MainMenu.Behaviors;

public class MenuBehavior
{
    public static readonly AttachedProperty<bool> UpdateCommandUiItemsProperty =
        AvaloniaProperty.RegisterAttached<MenuBehavior, MenuItem, bool>(
            "UpdateCommandUiItems", false);

    static MenuBehavior()
    {
        // 当属性改变时触发
        UpdateCommandUiItemsProperty.Changed.Subscribe(args =>
        {
            if (args.Sender is MenuItem menuItem && args.NewValue.HasValue && args.NewValue.Value)
            {
                menuItem.AddHandler(MenuItem.SubmenuOpenedEvent, OnSubmenuOpened);
                if (menuItem.IsSubMenuOpen)
                    OnSubmenuOpened(menuItem, new RoutedEventArgs());
            }
        });
    }

    public static void SetUpdateCommandUiItems(AvaloniaObject control, bool value)
    {
        control.SetValue(UpdateCommandUiItemsProperty, value);
    }

    public static bool GetUpdateCommandUiItems(AvaloniaObject control)
    {
        return control.GetValue(UpdateCommandUiItemsProperty);
    }

    private static void OnSubmenuOpened(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        var commandRouter = (App.Current as App).ServiceProvider.GetService<ICommandRouter>();
        foreach (var item in menuItem.Items.OfType<ICommandUiItem>().ToList())
            item.Update(commandRouter.GetCommandHandler(item.CommandDefinition));
    }
}