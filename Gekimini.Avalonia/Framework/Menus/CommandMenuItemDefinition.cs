using System;
using Avalonia.Input;
using Gekimini.Avalonia;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Microsoft.Extensions.DependencyInjection;

namespace Gemini.Framework.Menus;

public class CommandMenuItemDefinition<TCommandDefinition> : MenuItemDefinition
    where TCommandDefinition : CommandDefinitionBase
{
    public CommandMenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
        : base(group, sortOrder)
    {
    }

    public override CommandDefinitionBase CommandDefinition => field ??= (App.Current as App)
        ?.ServiceProvider
        .GetService<ICommandService>().GetCommandDefinition(typeof(TCommandDefinition));

    public override LocalizedString Text => CommandDefinition.Text;

    public override Uri IconSource => CommandDefinition.IconSource;

    public override KeyGesture KeyGesture => field ??= (App.Current as App)?.ServiceProvider
        .GetService<ICommandKeyGestureService>().GetPrimaryKeyGesture(CommandDefinition);
}