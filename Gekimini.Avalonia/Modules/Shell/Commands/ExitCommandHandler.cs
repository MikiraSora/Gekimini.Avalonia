using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class ExitCommandHandler : CommandHandlerBase<ExitCommandDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public ExitCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task Run(Command command)
    {
        var shell = _serviceProvider.GetService<IShell>();
        shell.Close();
        return Task.CompletedTask;
    }
}