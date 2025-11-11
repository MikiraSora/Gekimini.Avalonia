using Gekimini.Avalonia.Modules.UndoRedo.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemDefinition EditUndoMenuItem = new CommandMenuItemDefinition<UndoCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.EditUndoRedoMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition EditRedoMenuItem = new CommandMenuItemDefinition<RedoCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.EditUndoRedoMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition ViewHistoryMenuItem = new CommandMenuItemDefinition<ViewHistoryCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 5);
}