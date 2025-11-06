using Gekimini.Avalonia.Views;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar.Views;

public partial class StatusBarView : ViewBase, IStatusBarView
{
    public StatusBarView()
    {
        InitializeComponent();
        StatusBarManager.BindContainer(statusBarContainer);
    }

    public StatusBarManager StatusBarManager { get; } = new();
}