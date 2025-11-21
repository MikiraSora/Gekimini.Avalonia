using Gekimini.Avalonia.Modules.InternalTest.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.InternalTest;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemDefinition ViewInternalTestTool =
        new CommandMenuItemDefinition<ViewInternalTestToolCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 5);
}