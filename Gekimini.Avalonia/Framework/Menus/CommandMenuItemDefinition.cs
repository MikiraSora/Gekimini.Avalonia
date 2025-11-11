using System;
using Avalonia.Input;
using Gekimini.Avalonia;
using Gekimini.Avalonia.Framework.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Gemini.Framework.Menus;

public class CommandMenuItemDefinition<TCommandDefinition> : MenuItemDefinition
    where TCommandDefinition : CommandDefinitionBase
{
    private CommandDefinitionBase _commandDefinition;

    private KeyGesture _keyGesture;

    public CommandMenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
        : base(group, sortOrder)
    {
    }

    public override CommandDefinitionBase CommandDefinition => _commandDefinition ??= (App.Current as App)
        ?.ServiceProvider
        .GetService<ICommandService>().GetCommandDefinition(typeof(TCommandDefinition));

    public override string Text => CommandDefinition.Text;

    public override Uri IconSource => CommandDefinition.IconSource;

    public override KeyGesture KeyGesture => _keyGesture ??= (App.Current as App)?.ServiceProvider
        .GetService<ICommandKeyGestureService>().GetPrimaryKeyGesture(CommandDefinition);
}