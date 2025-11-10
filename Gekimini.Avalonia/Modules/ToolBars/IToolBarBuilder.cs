using Gekimini.Avalonia.Framework.ToolBars;

namespace Gekimini.Avalonia.Modules.ToolBars;

public interface IToolBarBuilder
{
    void BuildToolBars(IToolBars result);
    void BuildToolBar(ToolBarDefinition toolBarDefinition, IToolBar result);
}