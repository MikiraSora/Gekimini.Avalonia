using Gekimini.Avalonia.Example.Modules.InternalTest.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest;

public static class MenuDefinitions
{
    [RegisterStaticObject]
    public static MenuItemDefinition ViewInternalTestTool =
        new CommandMenuItemDefinition<ViewInternalTestToolCommandDefinition>(
            Avalonia.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 5);
}