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
    private readonly Action<object, Command, List<Command>> _populateMethod;
    private readonly Func<object, Command, Task> _runMethod;
    private readonly Action<object, Command> _updateMethod;

    private CommandHandlerWrapper(
        object commandHandler,
        Action<object, Command> updateMethod,
        Action<object, Command, List<Command>> populateMethod,
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

    private static void updateMethod(object arg1, Command arg2)
    {
        if (arg1 is ICommandHandler handler)
            handler.Update(arg2);
    }

    private static void populateMethod(object arg1, Command arg2, List<Command> arg3)
    {
        if (arg1 is ICommandListHandler handler)
            handler.Populate(arg2, arg3);
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

    public void Update(Command command)
    {
        if (_updateMethod != null)
            _updateMethod.Invoke(_commandHandler, command);
    }

    public void Populate(Command command, List<Command> commands)
    {
        if (_populateMethod == null)
            throw new InvalidOperationException("Populate can only be called for list-type commands.");
        _populateMethod.Invoke(_commandHandler, command, commands);
    }

    public async Task Run(Command command)
    {
        await _runMethod.Invoke(_commandHandler, command);
        CommandManager.InvalidateRequerySuggested("commandExecuted");
    }
}