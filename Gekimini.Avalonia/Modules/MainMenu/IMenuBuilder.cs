using Gemini.Framework.Menus;

namespace Gekimini.Avalonia.Modules.MainMenu
{
    public interface IMenuBuilder
    {
        void BuildMenuBar(MenuBarDefinition menuBarDefinition, IMenu result);
    }
}