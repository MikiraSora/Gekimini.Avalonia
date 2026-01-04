using Gekimini.Avalonia.Framework.ToolBars;
using Gekimini.Avalonia.Modules.UndoRedo.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo;

public static class ToolBarDefinitions
{
    [RegisterStaticObject]
    public static ToolBarItemGroupDefinition StandardUndoRedoToolBarGroup = new(
        ToolBars.ToolBarDefinitions.StandardToolBar, 10);

    [RegisterStaticObject]
    public static ToolBarItemDefinition UndoToolBarItem = new CommandToolBarItemDefinition<UndoCommandDefinition>(
        StandardUndoRedoToolBarGroup, 0);

    [RegisterStaticObject]
    public static ToolBarItemDefinition RedoToolBarItem = new CommandToolBarItemDefinition<RedoCommandDefinition>(
        StandardUndoRedoToolBarGroup, 1);
}