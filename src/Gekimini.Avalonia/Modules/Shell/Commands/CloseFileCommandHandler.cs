using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class CloseFileCommandHandler : CommandHandlerBase<CloseFileCommandDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public CloseFileCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task Update(Command command)
    {
        var shell = _serviceProvider.GetService<IShell>();
        command.Enabled = shell.ActiveDocument != null;
        return base.Update(command);
    }

    public override Task Run(Command command)
    {
        var shell = _serviceProvider.GetService<IShell>();
        return shell.CloseDocumentAsync(shell.ActiveDocument);
    }
}
