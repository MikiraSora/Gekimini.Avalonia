using Gekimini.Avalonia.Framework.ToolBars;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.ToolBars;

public static class ToolBarDefinitions
{
    [RegisterStaticObject<ToolBarDefinition>]
    public static ToolBarDefinition StandardToolBar = new(0, "res:ToolBarStandard");
}