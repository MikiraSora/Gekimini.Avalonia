using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Platforms.Services.Window;
using Gekimini.Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Window.ViewModels;

public partial class WindowViewModelBase : ViewModelBase
{
    [ObservableProperty]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial int DefaultLeftX { get; set; } = 200;

    [ObservableProperty]
    public partial int DefaultTopY { get; set; } = 200;

    [ObservableProperty]
    public partial ControlSize DefaultWidth { get; set; } = 400;

    [ObservableProperty]
    public partial ControlSize DefaultHeight { get; set; } = 600;

    protected Task TryCloseAsync(bool dialogResult)
    {
        return (Application.Current as App)?.ServiceProvider?.GetService<IWindowManager>()
            .TryCloseWindowAsync(this, dialogResult);
    }
}