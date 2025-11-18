using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class ViewFullScreenCommandHandler : CommandHandlerBase<ViewFullScreenCommandDefinition>
{
    private readonly ILogger<ViewFullScreenCommandHandler> _logger;

    public ViewFullScreenCommandHandler(ILogger<ViewFullScreenCommandHandler> logger)
    {
        _logger = logger;
    }

    public override Task Run(Command command)
    {
        //todo
        _logger.LogInformationEx("gugugu");
        return Task.CompletedTask;
    }
}