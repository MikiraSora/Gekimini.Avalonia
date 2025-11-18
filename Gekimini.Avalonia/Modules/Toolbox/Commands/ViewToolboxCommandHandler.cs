using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Toolbox.Commands;

[RegisterSingleton<ICommandHandler>]
public class ViewToolboxCommandHandler : CommandHandlerBase<ViewToolboxCommandDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public ViewToolboxCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task Run(Command command)
    {
        _serviceProvider.GetService<IShell>().ShowTool<IToolbox>();
        return Task.CompletedTask;
    }
}