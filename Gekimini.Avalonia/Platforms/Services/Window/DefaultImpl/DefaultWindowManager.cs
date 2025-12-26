using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Modules.Window.Views;
using Gekimini.Avalonia.Views;
using Iciclecreek.Avalonia.WindowManager;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Platforms.Services.Window.DefaultImpl;

[RegisterSingleton<IWindowManager>]
public partial class DefaultWindowManager : IWindowManager
{
    [GetServiceLazy]
    private partial ILogger<DefaultWindowManager> Logger { get; }

    [GetServiceLazy]
    private partial ViewLocator ViewLocator { get; }

    public Task ShowWindowAsync(WindowViewBase windowView)
    {
        return ShowWindowAsyncInternal(windowView, false);
    }

    public Task ShowDialogAsync(WindowViewBase windowView)
    {
        return ShowWindowAsyncInternal(windowView, true);
    }

    public Task TryCloseWindowAsync(WindowViewBase window, bool dialogResult)
    {
        window?.Close(dialogResult);
        return Task.CompletedTask;
    }

    public Task ShowWindowAsync(WindowViewModelBase windowViewModel)
    {
        var view = ViewLocator.Build(windowViewModel);
        if (view is not WindowViewBase windowView)
            throw new Exception(
                $"view type of viewModel {windowViewModel?.GetType().Name} must be subtype of WindowViewBase, but actual view type is {view?.GetType().Name}");

        return ShowWindowAsync(windowView);
    }

    public Task ShowDialogAsync(WindowViewModelBase windowViewModel)
    {
        var view = ViewLocator.Build(windowViewModel);
        if (view is not WindowViewBase windowView)
            throw new Exception(
                $"view type of viewModel {windowViewModel?.GetType().Name} must be subtype of WindowViewBase, but actual view type is {view?.GetType().Name}");

        return ShowDialogAsync(windowView);
    }

    public Task TryCloseWindowAsync(WindowViewModelBase windowViewModelBase, bool dialogResult)
    {
        if (FindWindowViewInCurrentWindows(windowViewModelBase) is { } windowView)
            return TryCloseWindowAsync(windowView, dialogResult);

        return Task.CompletedTask;
    }

    private async Task ShowWindowAsyncInternal(WindowViewBase window, bool isModel)
    {
        if (!TryGetCurrentWindowPanel(out var windowPanel))
        {
            Logger.LogErrorEx("WindowPanel not found in entity visual tree.");
            return;
        }

        if (isModel)
        {
            var visual = /*
                            windowPanel.ModalDialog as Visual ??
                            windowPanel.Windows.OfType<WindowViewBase>().FirstOrDefault(x => x.IsActive) as Visual ??
                         */
                windowPanel;
            await window.ShowDialog(visual);
        }
        else
            window.Show(windowPanel);
    }

    private WindowViewBase FindWindowViewInCurrentWindows(WindowViewModelBase windowViewModelBase)
    {
        if (!TryGetCurrentWindowPanel(out var windowPanel))
            return default;
        if (windowPanel.Windows.FirstOrDefault(x => x.DataContext == windowViewModelBase) is WindowViewBase
            windowView)
            return windowView;
        return default;
    }

    private bool TryGetCurrentWindowPanel(out WindowsPanel windowsPanel)
    {
        windowsPanel = default;

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            windowsPanel = singleView.MainView?.FindDescendantOfType<WindowsPanel>(true);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            windowsPanel ??= desktop.MainWindow?.FindDescendantOfType<WindowsPanel>(true);

        return windowsPanel != null;
    }
}