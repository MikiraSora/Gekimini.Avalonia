using System;
using System.Collections.Generic;
using System.Linq;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Commands;

[RegisterSingleton<ICommandService>]
public class CommandService : ICommandService
{
    private readonly IEnumerable<CommandDefinitionBase> _commandDefinitions;
    private readonly Dictionary<Type, CommandDefinitionBase> _commandDefinitionsLookup;
    private readonly ICommandRouter _commandRouter;
    private readonly Dictionary<CommandDefinitionBase, Command> _commands;
    private readonly Dictionary<Command, TargetableCommand> _targetableCommands;

    public CommandService(IEnumerable<CommandDefinitionBase> commandDefinitions, ICommandRouter commandRouter)
    {
        _commandDefinitions = commandDefinitions.ToArray();
        _commandRouter = commandRouter;
        _commandDefinitionsLookup = new Dictionary<Type, CommandDefinitionBase>();
        _commands = new Dictionary<CommandDefinitionBase, Command>();
        _targetableCommands = new Dictionary<Command, TargetableCommand>();
    }

    public CommandDefinitionBase GetCommandDefinition(Type commandDefinitionType)
    {
        CommandDefinitionBase commandDefinition;
        if (!_commandDefinitionsLookup.TryGetValue(commandDefinitionType, out commandDefinition))
            commandDefinition = _commandDefinitionsLookup[commandDefinitionType] =
                _commandDefinitions.First(x => x.GetType() == commandDefinitionType);
        return commandDefinition;
    }

    public Command GetCommand(CommandDefinitionBase commandDefinition)
    {
        Command command;
        if (!_commands.TryGetValue(commandDefinition, out command))
            command = _commands[commandDefinition] = new Command(commandDefinition);
        return command;
    }

    public TargetableCommand GetTargetableCommand(Command command)
    {
        TargetableCommand targetableCommand;
        if (!_targetableCommands.TryGetValue(command, out targetableCommand))
            targetableCommand = _targetableCommands[command] = new TargetableCommand(_commandRouter, command);
        return targetableCommand;
    }
}