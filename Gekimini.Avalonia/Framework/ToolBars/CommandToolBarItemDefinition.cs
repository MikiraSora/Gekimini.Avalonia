using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Framework.ToolBars;

public class CommandToolBarItemDefinition<TCommandDefinition> : ToolBarItemDefinition
    where TCommandDefinition : CommandDefinitionBase
{
    private CommandDefinitionBase _commandDefinition;
    private KeyGesture _keyGesture;

    public CommandToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder,
        ToolBarItemDisplay display = ToolBarItemDisplay.IconOnly)
        : base(group, sortOrder, display)
    {
    }

    public override CommandDefinitionBase CommandDefinition => _commandDefinition ??=
        (App.Current as App).ServiceProvider.GetService<ICommandService>()
        .GetCommandDefinition(typeof(TCommandDefinition));

    public override string Text => CommandDefinition.ToolTip;

    public override Uri IconSource => CommandDefinition.IconSource;

    public override KeyGesture KeyGesture => _keyGesture ??=
        (App.Current as App).ServiceProvider.GetService<ICommandKeyGestureService>()
        .GetPrimaryKeyGesture(_commandDefinition);
}