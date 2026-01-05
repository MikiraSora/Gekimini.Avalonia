using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Menus;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.MainMenu;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuBarDefinition MainMenuBar = new();

    [RegisterStaticObject]
    public static MenuDefinition FileMenu = new(MainMenuBar, 0, ProgramLanguages.B.FileMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileNewOpenMenuGroup = new(FileMenu, 0);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileCloseMenuGroup = new(FileMenu, 3);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileSaveMenuGroup = new(FileMenu, 6);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileExitOpenMenuGroup = new(FileMenu, 10);

    [RegisterStaticObject]
    public static MenuDefinition EditMenu = new(MainMenuBar, 1,
        ProgramLanguages.B.EditMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new(EditMenu, 0);

    [RegisterStaticObject]
    public static MenuDefinition ViewMenu = new(MainMenuBar, 2,
        ProgramLanguages.B.ViewMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition ViewToolsMenuGroup = new(ViewMenu, 0);

    [RegisterStaticObject]
    public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new(ViewMenu, 100);

    [RegisterStaticObject]
    public static MenuDefinition ToolsMenu = new(MainMenuBar, 10,
        ProgramLanguages.B.ToolsMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition ToolsOptionsMenuGroup = new(ToolsMenu, 100);

    [RegisterStaticObject]
    public static MenuDefinition WindowMenu = new(MainMenuBar, 20,
        ProgramLanguages.B.WindowMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new(WindowMenu, 10);

    [RegisterStaticObject]
    public static MenuDefinition HelpMenu = new(MainMenuBar, 30,
        ProgramLanguages.B.HelpMenuText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition HelpMenuGroup = new(HelpMenu, 10);
}