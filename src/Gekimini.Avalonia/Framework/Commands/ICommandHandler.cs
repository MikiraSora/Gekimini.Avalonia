using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Framework.Commands;

public interface ICommandHandler<out TCommandDefinition> : ICommandHandler
    where TCommandDefinition : CommandDefinition
{
}

public interface ICommandListHandler<out TCommandDefinition> : ICommandListHandler
    where TCommandDefinition : CommandListDefinition
{
}

public interface ICommandHandler
{
    IEnumerable<Type> SupportCommandDefinitionTypes { get; }
    Task Update(Command command);
    Task Run(Command command);
}

public interface ICommandListHandler : ICommandHandler
{
    Task Populate(Command command, List<Command> commands);
}

public abstract class CommandHandlerBase<TCommandDefinition> : ICommandHandler<TCommandDefinition>
    where TCommandDefinition : CommandDefinition
{
    public virtual Task Update(Command command)
    {
        return Task.CompletedTask;
    }

    public abstract Task Run(Command command);

    public virtual IEnumerable<Type> SupportCommandDefinitionTypes { get; } =
    [
        typeof(TCommandDefinition)
    ];
}

public abstract class CommandListHandlerBase<TCommandListDefinition> : ICommandListHandler<TCommandListDefinition>
    where TCommandListDefinition : CommandListDefinition
{
    public virtual IEnumerable<Type> SupportCommandDefinitionTypes { get; } =
    [
        typeof(TCommandListDefinition)
    ];

    public virtual Task Update(Command command)
    {
        return Task.CompletedTask;
    }

    public virtual Task Run(Command command)
    {
        return Task.CompletedTask;
    }

    public abstract Task Populate(Command command, List<Command> commands);
}