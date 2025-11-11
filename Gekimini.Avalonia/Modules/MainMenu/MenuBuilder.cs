using System;
using System.Collections.Generic;
using System.Linq;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;
using Gemini.Framework.Menus;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.MainMenu;

[RegisterSingleton<IMenuBuilder>]
public class MenuBuilder : IMenuBuilder
{
    private readonly ICommandService _commandService;
    private readonly MenuItemGroupDefinition[] _excludeMenuItemGroups;
    private readonly MenuItemDefinition[] _excludeMenuItems;
    private readonly MenuDefinition[] _excludeMenus;
    private readonly MenuBarDefinition[] _menuBars;
    private readonly MenuItemGroupDefinition[] _menuItemGroups;
    private readonly MenuItemDefinition[] _menuItems;
    private readonly MenuDefinition[] _menus;
    private readonly IServiceProvider _serviceProvider;

    public MenuBuilder(
        ICommandService commandService,
        IEnumerable<MenuBarDefinition> menuBars,
        IEnumerable<MenuDefinition> menus,
        IEnumerable<MenuItemGroupDefinition> menuItemGroups,
        IEnumerable<MenuItemDefinition> menuItems,
        IEnumerable<ExcludeMenuDefinition> excludeMenus,
        IEnumerable<ExcludeMenuItemGroupDefinition> excludeMenuItemGroups,
        IEnumerable<ExcludeMenuItemDefinition> excludeMenuItems,
        IServiceProvider serviceProvider)
    {
        _commandService = commandService;
        _serviceProvider = serviceProvider;
        _menuBars = menuBars.ToArray();
        _menus = menus.ToArray();
        _menuItemGroups = menuItemGroups.ToArray();
        _menuItems = menuItems.ToArray();
        _excludeMenus = excludeMenus.Select(x => x.MenuDefinitionToExclude).ToArray();
        _excludeMenuItemGroups = excludeMenuItemGroups.Select(x => x.MenuItemGroupDefinitionToExclude).ToArray();
        _excludeMenuItems = excludeMenuItems.Select(x => x.MenuItemDefinitionToExclude).ToArray();
    }

    public void BuildMenuBar(MenuBarDefinition menuBarDefinition, IMenu result)
    {
        var menus = _menus
            .Where(x => x.MenuBar == menuBarDefinition)
            .Where(x => !_excludeMenus.Contains(x))
            .OrderBy(x => x.SortOrder);

        foreach (var menu in menus)
        {
            var menuModel = new TextMenuItemViewModel(menu);
            AddGroupsRecursive(menu, menuModel);
            if (menuModel.Children.Any())
                result.MenuItems.Add(menuModel);
        }
    }

    private void AddGroupsRecursive(MenuDefinitionBase menu, StandardMenuItemViewModel menuModel)
    {
        var groups = _menuItemGroups
            .Where(x => x.Parent == menu)
            .Where(x => !_excludeMenuItemGroups.Contains(x))
            .OrderBy(x => x.SortOrder)
            .ToList();

        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var menuItems = _menuItems
                .Where(x => x.Group == group)
                .Where(x => !_excludeMenuItems.Contains(x))
                .OrderBy(x => x.SortOrder);

            foreach (var menuItem in menuItems)
            {
                var menuItemModel = menuItem.CommandDefinition != null
                    ? new CommandMenuItemViewModel(_commandService.GetCommand(menuItem.CommandDefinition), menuModel,
                        _serviceProvider)
                    : (StandardMenuItemViewModel) new TextMenuItemViewModel(menuItem);
                AddGroupsRecursive(menuItem, menuItemModel);
                menuModel.Add(menuItemModel);
            }

            if (i < groups.Count - 1 && menuItems.Any())
                menuModel.Add(new SeparatorItemViewModel());
        }
    }
}