using Gekimini.Avalonia.Framework.ToolBars;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell;

public static class ToolBarDefinitions
{
    [RegisterStaticObject<ToolBarItemGroupDefinition>]
    public static ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new(
        Gekimini.Avalonia.Modules.ToolBars.ToolBarDefinitions.StandardToolBar, 8);

    [RegisterStaticObject<ToolBarItemDefinition>]
    public static ToolBarItemDefinition OpenFileToolBarItem =
        new CommandToolBarItemDefinition<OpenFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 0);

    [RegisterStaticObject<ToolBarItemDefinition>]
    public static ToolBarItemDefinition SaveFileToolBarItem =
        new CommandToolBarItemDefinition<SaveFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 2);

    [RegisterStaticObject<ToolBarItemDefinition>]
    public static ToolBarItemDefinition SaveAllFilesToolBarItem =
        new CommandToolBarItemDefinition<SaveAllFilesCommandDefinition>(
            StandardOpenSaveToolBarGroup, 4);
}