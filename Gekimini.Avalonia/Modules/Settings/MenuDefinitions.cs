using Gekimini.Avalonia.Modules.Settings.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Settings;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static readonly MenuItemDefinition OpenSettingsMenuItem =
        new CommandMenuItemDefinition<OpenSettingsCommandDefinition>(
            MainMenu.MenuDefinitions.ToolsOptionsMenuGroup, 0);
}