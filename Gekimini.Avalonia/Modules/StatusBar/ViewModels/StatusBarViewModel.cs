using Avalonia.Controls;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar.ViewModels;

[RegisterSingleton<IStatusBar>]
public class StatusBarViewModel : ViewModelBase, IStatusBar
{
    private IStatusBarView statusBarView;

    public StatusBarManager StatusBarManager => statusBarView.StatusBarManager;

    public override void OnViewAfterLoaded(IView view)
    {
        base.OnViewAfterLoaded(view);
        statusBarView = view as IStatusBarView;
    }

    public override void OnViewBeforeUnload(IView view)
    {
        base.OnViewBeforeUnload(view);
        statusBarView = null;
    }
}