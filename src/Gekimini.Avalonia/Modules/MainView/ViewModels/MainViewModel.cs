using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Iciclecreek.Avalonia.WindowManager;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.MainView.ViewModels;

[RegisterSingleton<IMainView>]
public partial class MainViewModel : ViewModelBase, IMainView
{
    [GetServiceLazy]
    public partial ILogger<MainViewModel> Logger { get; }

    [GetServiceLazy]
    public partial ICommandKeyGestureService KeyGestureService { get; }

    [GetServiceLazy]
    public partial IShell Shell { get; }

    public override void OnViewAfterLoaded(IView view)
    {
        if (view is Control control)
        {
            var rootVisual = TopLevel.GetTopLevel(control);
            KeyGestureService.BindKeyGestures(rootVisual);
        }

        base.OnViewAfterLoaded(view);
    }
}