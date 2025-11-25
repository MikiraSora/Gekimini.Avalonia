using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework.ToolBars;

public abstract class ToolBarItemDefinition
{
    protected ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display)
    {
        Group = group;
        SortOrder = sortOrder;
        Display = display;
    }

    public ToolBarItemGroupDefinition Group { get; }

    public int SortOrder { get; }

    public ToolBarItemDisplay Display { get; }

    public abstract LocalizedString Text { get; }
    public abstract Uri IconSource { get; }
    public abstract KeyGesture KeyGesture { get; }
    public abstract CommandDefinitionBase CommandDefinition { get; }
}

public enum ToolBarItemDisplay
{
    IconOnly,
    IconAndText
}