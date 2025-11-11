using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.MainView.ViewModels;

[RegisterSingleton<IMainView>]
public partial class MainViewModel : ViewModelBase, IMainView
{
    private readonly ILogger logger;
    private readonly ICommandKeyGestureService _keyGestureService;

    [ObservableProperty]
    private IShell shell;

    public MainViewModel(ILogger<MainViewModel> logger, IShell shell,ICommandKeyGestureService keyGestureService)
    {
        this.logger = logger;
        _keyGestureService = keyGestureService;
        Shell = shell;
    }

    public override void OnViewAfterLoaded(Control view)
    {
        var rootVisual = TopLevel.GetTopLevel(view);
        _keyGestureService.BindKeyGestures(rootVisual);
        base.OnViewAfterLoaded(view);
    }
}