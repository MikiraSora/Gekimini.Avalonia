using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Framework.Commands;

/// <summary>
///     Wraps a generic ICommandHandler&lt;T&gt; or ICommandListHandler&lt;T&gt;
///     and allows easy calling of generic interface methods.
/// </summary>
public sealed class CommandHandlerWrapper
{
    private readonly object _commandHandler;
    private readonly Func<object, Command, List<Command>, Task> _populateMethod;
    private readonly Func<object, Command, Task> _runMethod;
    private readonly Func<object, Command, Task> _updateMethod;

    private CommandHandlerWrapper(
        object commandHandler,
        Func<object, Command, Task> updateMethod,
        Func<object, Command, List<Command>, Task> populateMethod,
        Func<object, Command, Task> runMethod)
    {
        _commandHandler = commandHandler;
        _updateMethod = updateMethod;
        _populateMethod = populateMethod;
        _runMethod = runMethod;
    }

    public static CommandHandlerWrapper FromCommandHandler(
        ICommandHandler commandHandler)
    {
        return new CommandHandlerWrapper(commandHandler, updateMethod, null, runMethod);
    }

    private static Task updateMethod(object arg1, Command arg2)
    {
        if (arg1 is ICommandHandler handler)
            return handler.Update(arg2);
        return Task.CompletedTask;
    }

    private static Task populateMethod(object arg1, Command arg2, List<Command> arg3)
    {
        if (arg1 is ICommandListHandler handler)
            return handler.Populate(arg2, arg3);
        return Task.CompletedTask;
    }

    private static Task runMethod(object arg1, Command arg2)
    {
        if (arg1 is ICommandHandler handler)
            return handler.Run(arg2);
        return Task.CompletedTask;
    }

    public static CommandHandlerWrapper FromCommandListHandler(
        ICommandListHandler commandListHandler)
    {
        return new CommandHandlerWrapper(commandListHandler, updateMethod, populateMethod, runMethod);
    }

    public Task Update(Command command)
    {
        return _updateMethod?.Invoke(_commandHandler, command) ?? Task.CompletedTask;
    }

    public Task Populate(Command command, List<Command> commands)
    {
        if (_populateMethod == null)
            throw new InvalidOperationException("Populate can only be called for list-type commands.");
        return _populateMethod.Invoke(_commandHandler, command, commands);
    }

    public async Task Run(Command command)
    {
        await _runMethod.Invoke(_commandHandler, command);
        CommandManager.InvalidateRequerySuggested("commandExecuted");
    }
}
