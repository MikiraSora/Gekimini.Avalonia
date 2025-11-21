using Gekimini.Avalonia.Framework.RecentFiles.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileOpenRecentMenuGroup = new(Modules.MainMenu.MenuDefinitions.FileMenu, 9);

    [RegisterStaticObject]
    public static MenuItemDefinition FileRecentFilesMenuItem =
        new CommandMenuItemDefinition<RecentFilesCommandDefinition>(
            FileOpenRecentMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileRecentFilesCascadeGroup = new(
        FileRecentFilesMenuItem, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileOpenRecentMenuItemList =
        new CommandMenuItemDefinition<OpenRecentFileCommandListDefinition>(
            FileRecentFilesCascadeGroup, 0);
}