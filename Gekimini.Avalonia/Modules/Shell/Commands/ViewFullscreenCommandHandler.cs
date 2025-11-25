using System.Threading.Tasks;
using Avalonia.Controls;
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

    public override void Update(Command command)
    {
        base.Update(command);

        command.Checked = PlatformMainWindow.WindowState switch
        {
            WindowState.FullScreen => true,
            _ => false
        };
    }

    public override Task Run(Command command)
    {
        PlatformMainWindow.WindowState = PlatformMainWindow.WindowState switch
        {
            WindowState.Normal => WindowState.FullScreen,
            _ => WindowState.Normal
        };

        return Task.CompletedTask;
    }
}