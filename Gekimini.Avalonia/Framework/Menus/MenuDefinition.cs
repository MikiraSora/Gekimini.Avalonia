using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gemini.Framework.Menus;

namespace Gekimini.Avalonia.Framework.Menus;

public class MenuDefinition : MenuDefinitionBase
{
    private readonly int _sortOrder;
    private readonly LocalizedString _text;

    public MenuDefinition(MenuBarDefinition menuBar, int sortOrder, LocalizedString text)
    {
        MenuBar = menuBar;
        _sortOrder = sortOrder;
        _text = text;
    }

    public MenuBarDefinition MenuBar { get; }

    public override int SortOrder => _sortOrder;

    public override LocalizedString Text => _text;

    public override Uri IconSource => null;

    public override KeyGesture KeyGesture => null;

    public override CommandDefinitionBase CommandDefinition => null;
}