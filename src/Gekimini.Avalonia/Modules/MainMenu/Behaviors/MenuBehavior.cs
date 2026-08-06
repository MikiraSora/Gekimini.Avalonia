using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
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

    private static async void OnSubmenuOpened(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is not MenuItem menuItem)
                return;

            var commandRouter = (App.Current as App)?.ServiceProvider.GetService<ICommandRouter>();
            if (commandRouter is null)
                return;

            foreach (var item in menuItem.Items.OfType<ICommandUiItem>().ToList())
            {
                var commandHandler = commandRouter.GetCommandHandler(item.CommandDefinition);
                if (commandHandler is not null)
                    await item.Update(commandHandler);
            }
        }
        catch (Exception exception)
        {
            Trace.TraceError($"Failed to update submenu command state: {exception}");
        }
    }
}
