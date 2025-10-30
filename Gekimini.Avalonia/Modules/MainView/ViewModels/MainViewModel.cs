using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework.Services;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.MainView.ViewModels;

[RegisterSingleton<IMainView>]
public partial class MainViewModel : ViewModelBase, IMainView
{
    private readonly ILogger logger;

    [ObservableProperty]
    private IShell shell;

    public MainViewModel(ILogger<MainViewModel> logger, IShell shell)
    {
        this.logger = logger;
        Shell = shell;
    }

    public override void OnViewAfterLoaded(Control view)
    {
        base.OnViewAfterLoaded(view);
    }
}