using System.Threading.Tasks;
using Avalonia;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class ExitCommandHandler : CommandHandlerBase<ExitCommandDefinition>
{
    public override async Task Run(Command command)
    {
        if (Application.Current is App app)
            await app.TryExit();
    }
}