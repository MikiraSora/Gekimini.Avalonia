using Gekimini.Avalonia.Modules.Toolbox.Commands;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Toolbox
{
    public static class MenuDefinitions
    {
        [RegisterStaticObject]
        public static MenuItemDefinition ViewToolboxMenuItem = new CommandMenuItemDefinition<ViewToolboxCommandDefinition>(
            MainMenu.MenuDefinitions.ViewToolsMenuGroup, 4);
    }
}