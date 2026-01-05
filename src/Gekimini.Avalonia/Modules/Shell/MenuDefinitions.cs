using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemDefinition FileNewMenuItem = new TextMenuItemDefinition(
        MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0,
        ProgramLanguages.B.FileNewCommandText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileNewCascadeGroup = new(
        FileNewMenuItem, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileNewMenuItemList = new CommandMenuItemDefinition<NewFileCommandListDefinition>(
        FileNewCascadeGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileOpenMenuItem = new TextMenuItemDefinition(
        MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0,
        ProgramLanguages.B.FileOpenCommandText.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition FileOpenCascadeGroup = new(
        FileOpenMenuItem, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileOpenMenuItemList =
        new CommandMenuItemDefinition<OpenFileCommandListDefinition>(
            FileOpenCascadeGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileCloseMenuItem = new CommandMenuItemDefinition<CloseFileCommandDefinition>(
        MainMenu.MenuDefinitions.FileCloseMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveMenuItem = new CommandMenuItemDefinition<SaveFileCommandDefinition>(
        MainMenu.MenuDefinitions.FileSaveMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveAsMenuItem = new CommandMenuItemDefinition<SaveFileAsCommandDefinition>(
        MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileSaveAllMenuItem = new CommandMenuItemDefinition<SaveAllFilesCommandDefinition>(
        MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommandDefinition>(
        MainMenu.MenuDefinitions.FileExitOpenMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition ViewFullscreenItem =
        new CommandMenuItemDefinition<ViewFullScreenCommandDefinition>(
            MainMenu.MenuDefinitions.ViewPropertiesMenuGroup, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition AubotGekiminiMenuItem =
        new CommandMenuItemDefinition<AboutGekiminiCommandDefinition>(
            MainMenu.MenuDefinitions.HelpMenuGroup, 0);

    #region Window

    [RegisterStaticObject]
    public static MenuItemDefinition WindowDocumentList =
        new CommandMenuItemDefinition<SwitchToDocumentCommandListDefinition>(
            MainMenu.MenuDefinitions.WindowDocumentListMenuGroup, 0);


    [RegisterStaticObject]
    public static MenuItemGroupDefinition LayoutMenuGroup = new(MainMenu.MenuDefinitions.WindowMenu, 1);

    [RegisterStaticObject]
    public static MenuItemDefinition LayoutMenuItem =
        new TextMenuItemDefinition(LayoutMenuGroup, 0, ProgramLanguages.B.Layout.ToLocalizedString());

    [RegisterStaticObject]
    public static MenuItemGroupDefinition LayoutMenuItemGroup = new(
        LayoutMenuItem, 0);

    [RegisterStaticObject]
    public static MenuItemDefinition ResetLayoutMenuItem =
        new CommandMenuItemDefinition<ResetLayoutCommandDefinition>(LayoutMenuItemGroup, 0);

    #endregion
}