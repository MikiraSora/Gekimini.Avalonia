using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Modules.Window.Views;
using Gekimini.Avalonia.Platforms.Services.Settings;
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
    private partial ISettingManager SettingManager { get; }

    [GetServiceLazy]
    private partial ViewLocator ViewLocator { get; }

    public Task ShowWindowAsync(WindowViewBase windowView)
    {
        ArgumentNullException.ThrowIfNull(windowView);
        return RunOnUiThreadAsync(async () =>
        {
            await ShowWindowAsyncInternal(windowView, false);
        });
    }

    public Task<bool?> ShowDialogAsync(WindowViewBase windowView)
    {
        ArgumentNullException.ThrowIfNull(windowView);
        return RunOnUiThreadAsync(() => ShowWindowAsyncInternal(windowView, true));
    }

    public Task TryCloseWindowAsync(WindowViewBase window, bool dialogResult)
    {
        return RunOnUiThreadAsync(() => window?.CloseAsync(dialogResult) ?? Task.CompletedTask);
    }

    public Task ShowWindowAsync(WindowViewModelBase windowViewModel)
    {
        ArgumentNullException.ThrowIfNull(windowViewModel);
        return RunOnUiThreadAsync(async () =>
        {
            var windowView = BuildWindow(windowViewModel);
            await ShowWindowAsyncInternal(windowView, false);
        });
    }

    public Task<bool?> ShowDialogAsync(WindowViewModelBase windowViewModel)
    {
        ArgumentNullException.ThrowIfNull(windowViewModel);
        return RunOnUiThreadAsync(() =>
        {
            var windowView = BuildWindow(windowViewModel);
            return ShowWindowAsyncInternal(windowView, true);
        });
    }

    public Task TryCloseWindowAsync(WindowViewModelBase windowViewModelBase, bool dialogResult)
    {
        ArgumentNullException.ThrowIfNull(windowViewModelBase);
        return RunOnUiThreadAsync(() =>
            FindWindowViewInCurrentWindows(windowViewModelBase)?.CloseAsync(dialogResult) ?? Task.CompletedTask);
    }

    private async Task<bool?> ShowWindowAsyncInternal(WindowViewBase window, bool isModel)
    {
        Dispatcher.UIThread.VerifyAccess();

        if (!TryGetCurrentWindowPanel(out var windowPanel))
        {
            Logger.LogErrorEx("WindowPanel not found in entity visual tree.");
            return null;
        }

        RestoreWindowPositionAndSize(window);
        AdjustWindowPositionAndSize(window);

        window.Closed -= WindowOnClosed;
        window.Closed += WindowOnClosed;

        if (isModel)
        {
            var visual = windowPanel.ModalDialog as Visual ??
                windowPanel.Windows.OfType<WindowViewBase>().FirstOrDefault(x => x.IsActive) as Visual ??
                windowPanel;
            return await window.ShowDialog<bool?>(visual);
        }
        else
        {
            window.Show(windowPanel);
            return null;
        }
    }

    private void WindowOnClosed(object sender, EventArgs e)
    {
        Dispatcher.UIThread.VerifyAccess();
        SaveWindowPositionAndSize(sender as WindowViewBase);
    }

    private WindowViewBase BuildWindow(WindowViewModelBase windowViewModel)
    {
        Dispatcher.UIThread.VerifyAccess();

        var view = ViewLocator.Build(windowViewModel);
        if (view is not WindowViewBase windowView)
            throw new Exception(
                $"view type of viewModel {windowViewModel.GetType().Name} must be subtype of WindowViewBase, but actual view type is {view?.GetType().Name}");

        return windowView;
    }

    private void RestoreWindowPositionAndSize(WindowViewBase windowView)
    {
        if (windowView is null)
            return;

        var setting = SettingManager.GetSetting(WindowPositionSizeSetting.JsonTypeInfo);

        if (!setting.WindowPositionSizeMap.TryGetValue(windowView.GetType().FullName!, out var windowPositionSizeMap))
            return;

        windowView.Position = new PixelPoint((int) windowPositionSizeMap.LeftX, (int) windowPositionSizeMap.TopY);
        windowView.Width = windowPositionSizeMap.Width.Value;
        windowView.Height = windowPositionSizeMap.Height.Value;
    }

    private void SaveWindowPositionAndSize(WindowViewBase windowView)
    {
        if (windowView is null)
            return;

        var setting = SettingManager.GetSetting(WindowPositionSizeSetting.JsonTypeInfo);
        var controlPositionSize = new ControlPositionSize(windowView.Position.X, windowView.Position.Y,
            windowView.Bounds.Width, windowView.Bounds.Height);
        setting.WindowPositionSizeMap[windowView.GetType().FullName!] = controlPositionSize;
        SettingManager.SaveSetting(setting, WindowPositionSizeSetting.JsonTypeInfo);
    }

    private void AdjustWindowPositionAndSize(WindowViewBase windowView)
    {
        if (windowView is null)
            return;

        if (!TryGetCurrentWindowPanel(out var windowPanel))
            return;

        windowView.AdjustWindowPositionAndSize(windowPanel);
    }

    private WindowViewBase FindWindowViewInCurrentWindows(WindowViewModelBase windowViewModelBase)
    {
        Dispatcher.UIThread.VerifyAccess();

        if (!TryGetCurrentWindowPanel(out var windowPanel))
            return default;
        if (windowPanel.Windows.FirstOrDefault(x => x.DataContext == windowViewModelBase) is WindowViewBase
            windowView)
            return windowView;
        return default;
    }

    private bool TryGetCurrentWindowPanel(out WindowsPanel windowsPanel)
    {
        Dispatcher.UIThread.VerifyAccess();

        windowsPanel = default;

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            windowsPanel = singleView.MainView?.FindDescendantOfType<WindowsPanel>(true);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            windowsPanel ??= desktop.MainWindow?.FindDescendantOfType<WindowsPanel>(true);

        return windowsPanel != null;
    }

    private static async Task RunOnUiThreadAsync(Func<Task> action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            await action();
        else
            await Dispatcher.UIThread.InvokeAsync(action);
    }

    private static async Task<T> RunOnUiThreadAsync<T>(Func<Task<T>> action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            return await action();

        return await Dispatcher.UIThread.InvokeAsync(action);
    }
}
