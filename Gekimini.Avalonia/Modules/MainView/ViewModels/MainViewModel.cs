using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.EmbeddedWindows;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
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
    
    [GetServiceLazy]
    public partial IEmbeddedWindow EmbeddedWindow { get; }

    public override void OnViewAfterLoaded(Control view)
    {
        var rootVisual = TopLevel.GetTopLevel(view);
        KeyGestureService.BindKeyGestures(rootVisual);
        base.OnViewAfterLoaded(view);
    }
}