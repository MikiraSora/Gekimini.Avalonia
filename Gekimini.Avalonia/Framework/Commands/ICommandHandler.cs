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
    void Update(Command command);
    Task Run(Command command);
}

public interface ICommandListHandler : ICommandHandler
{
    void Populate(Command command, List<Command> commands);
}

public abstract class CommandHandlerBase<TCommandDefinition> : ICommandHandler<TCommandDefinition>
    where TCommandDefinition : CommandDefinition
{
    public virtual void Update(Command command)
    {
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

    public virtual void Update(Command command)
    {
    }

    public virtual Task Run(Command command)
    {
        return Task.CompletedTask;
    }

    public abstract void Populate(Command command, List<Command> commands);
}