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
}