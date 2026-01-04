using System.Threading.Tasks;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Platforms.Services.MainWindow;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public partial class ViewFullScreenCommandHandler : CommandHandlerBase<ViewFullScreenCommandDefinition>
{
    [GetServiceLazy]
    private partial IPlatformMainWindow PlatformMainWindow { get; }

    public override Task Run(Command command)
    {
        PlatformMainWindow.IsFullScreen = !PlatformMainWindow.IsFullScreen;
        return Task.CompletedTask;
    }
}