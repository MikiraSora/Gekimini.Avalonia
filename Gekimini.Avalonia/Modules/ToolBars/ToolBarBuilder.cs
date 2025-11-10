using System.Collections.Generic;
using System.Linq;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.ToolBars;
using Gekimini.Avalonia.Modules.ToolBars.Models;
using Gekimini.Avalonia.Modules.ToolBars.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.ToolBars;

[RegisterSingleton<IToolBarBuilder>]
public class ToolBarBuilder : IToolBarBuilder
{
    private readonly ICommandService _commandService;
    private readonly ToolBarItemGroupDefinition[] _toolBarItemGroups;
    private readonly ToolBarItemDefinition[] _toolBarItems;
    private readonly ToolBarDefinition[] _toolBars;

    public ToolBarBuilder(
        ICommandService commandService,
        IEnumerable<ToolBarDefinition> toolBars,
        IEnumerable<ToolBarItemGroupDefinition> toolBarItemGroups,
        IEnumerable<ToolBarItemDefinition> toolBarItems,
        IEnumerable<ExcludeToolBarDefinition> excludeToolBars,
        IEnumerable<ExcludeToolBarItemGroupDefinition> excludeToolBarItemGroups,
        IEnumerable<ExcludeToolBarItemDefinition> excludeToolBarItems)
    {
        _commandService = commandService;
        _toolBars = toolBars
            .Where(x => !excludeToolBars.Select(y => y.ToolBarDefinitionToExclude).Contains(x))
            .ToArray();
        _toolBarItemGroups = toolBarItemGroups
            .Where(x => !excludeToolBarItemGroups.Select(y => y.ToolBarItemGroupDefinitionToExclude).Contains(x))
            .ToArray();
        _toolBarItems = toolBarItems
            .Where(x => !excludeToolBarItems.Select(y => y.ToolBarItemDefinitionToExclude).Contains(x))
            .ToArray();
    }

    public void BuildToolBars(IToolBars result)
    {
        var toolBars = _toolBars.OrderBy(x => x.SortOrder);

        foreach (var toolBar in toolBars)
        {
            var toolBarModel = new ToolBarModel();
            BuildToolBar(toolBar, toolBarModel);
            if (toolBarModel.Any())
                result.Items.Add(toolBarModel);
        }
    }

    public void BuildToolBar(ToolBarDefinition toolBarDefinition, IToolBar result)
    {
        var groups = _toolBarItemGroups
            .Where(x => x.ToolBar == toolBarDefinition)
            .OrderBy(x => x.SortOrder)
            .ToList();

        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var toolBarItems = _toolBarItems
                .Where(x => x.Group == group)
                .OrderBy(x => x.SortOrder);

            foreach (var toolBarItem in toolBarItems)
                result.Add(new CommandToolBarItemViewModel(toolBarItem,
                    _commandService.GetCommand(toolBarItem.CommandDefinition), result));
        }
    }
}