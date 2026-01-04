using Gekimini.Avalonia.Views;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar;

public interface IStatusBarView : IView
{
    StatusBarManager StatusBarManager { get; }
}