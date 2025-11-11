using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Gemini.Framework.Menus;
using Gemini.Modules.Shell.Commands;
using Injectio.Attributes;

namespace Gemini.Modules.Shell;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemDefinition FileNewMenuItem = new TextMenuItemDefinition(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0, Resources.FileNewCommandText);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileNewCascadeGroup = new(
        FileNewMenuItem, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileNewMenuItemList = new CommandMenuItemDefinition<NewFileCommandListDefinition>(
        FileNewCascadeGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileOpenMenuItem = new CommandMenuItemDefinition<OpenFileCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileCloseMenuItem = new CommandMenuItemDefinition<CloseFileCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileCloseMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveMenuItem = new CommandMenuItemDefinition<SaveFileCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveAsMenuItem = new CommandMenuItemDefinition<SaveFileAsCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveAllMenuItem = new CommandMenuItemDefinition<SaveAllFilesCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommandDefinition>(
        Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.FileExitOpenMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition WindowDocumentList =
        new CommandMenuItemDefinition<SwitchToDocumentCommandListDefinition>(
            Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.WindowDocumentListMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition ViewFullscreenItem =
        new CommandMenuItemDefinition<ViewFullScreenCommandDefinition>(
            Gekimini.Avalonia.Modules.MainMenu.MenuDefinitions.ViewPropertiesMenuGroup, 0);
}