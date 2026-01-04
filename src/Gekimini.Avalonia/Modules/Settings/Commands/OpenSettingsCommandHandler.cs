using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Settings.ViewModels;
using Gekimini.Avalonia.Platforms.Services.Window;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Settings.Commands;

[RegisterSingleton<ICommandHandler>]
public class OpenSettingsCommandHandler : CommandHandlerBase<OpenSettingsCommandDefinition>
{
    private readonly IServiceProvider serviceProvider;

    public OpenSettingsCommandHandler(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public override async Task Run(Command command)
    {
        await serviceProvider.GetService<IWindowManager>()
            .ShowDialogAsync(serviceProvider.GetService<SettingsViewModel>());
    }
}