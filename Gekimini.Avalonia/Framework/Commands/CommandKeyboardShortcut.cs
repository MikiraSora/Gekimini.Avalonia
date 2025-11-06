using System;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Framework.Commands;

public abstract class CommandKeyboardShortcut
{
    private readonly Func<CommandDefinitionBase> _commandDefinition;

    protected CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder,
        Func<CommandDefinitionBase> commandDefinition)
    {
        _commandDefinition = commandDefinition;
        KeyGesture = keyGesture;
        SortOrder = sortOrder;
    }

    public CommandDefinitionBase CommandDefinition => _commandDefinition();

    public KeyGesture KeyGesture { get; }

    public int SortOrder { get; }
}

public class CommandKeyboardShortcut<TCommandDefinition> : CommandKeyboardShortcut
    where TCommandDefinition : CommandDefinition
{
    public CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder = 5)
        : base(keyGesture, sortOrder,
            () => (App.Current as App)?.ServiceProvider.GetService<ICommandService>()
                .GetCommandDefinition(typeof(TCommandDefinition)))
    {
    }
}