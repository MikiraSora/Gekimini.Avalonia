using System.Threading.Tasks;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Modules.EmbeddedWindows;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Platforms.Services.Window.DefaultImpl;

[RegisterSingleton<IWindowManager>]
public partial class DefaultWindowManager : IWindowManager
{
    [GetServiceLazy]
    private partial IEmbeddedWindow EmbeddedWindow { get; }

    public Task ShowWindowAsync(WindowViewModelBase windowViewModel)
    {
        EmbeddedWindow.AddWindow(windowViewModel);
        return Task.CompletedTask;
    }

    public Task TryCloseWindowAsync(WindowViewModelBase windowViewModelBase, bool dialogResult)
    {
        EmbeddedWindow.RemoveWindow(windowViewModelBase);
        return Task.CompletedTask;
    }
}