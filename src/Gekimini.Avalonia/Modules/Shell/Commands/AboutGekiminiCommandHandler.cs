using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell.ViewModels.Windows;
using Gekimini.Avalonia.Platforms.Services.Window;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class AboutGekiminiCommandHandler : CommandHandlerBase<AboutGekiminiCommandDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public AboutGekiminiCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task Run(Command command)
    {
        var windowManager = _serviceProvider.GetService<IWindowManager>();
        return windowManager.ShowWindowAsync(_serviceProvider.Resolve<AboutGekiminiWindowViewModel>());
    }
}