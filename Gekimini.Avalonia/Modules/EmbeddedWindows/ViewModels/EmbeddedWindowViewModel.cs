using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.EmbeddedWindows.ViewModels;

[RegisterSingleton<IEmbeddedWindow>]
public partial class EmbeddedWindowViewModel : ViewModelBase, IEmbeddedWindow
{
    private readonly ISettingManager settingManager;
    private readonly WindowPositionSizeSetting windowPositionSizeSetting;
    private IView view;

    public EmbeddedWindowViewModel(ISettingManager settingManager)
    {
        this.settingManager = settingManager;
        windowPositionSizeSetting = settingManager.GetSetting(WindowPositionSizeSetting.JsonTypeInfo);
    }

    [GetServiceLazy]
    private partial ILogger<EmbeddedWindowViewModel> Logger { get; }

    public ObservableCollection<WindowViewModelWrapper> WindowViewModelWrappers { get; } = [];
    public ObservableCollection<WindowViewModelWrapper> MinimizedWindowViewModelWrappers { get; } = [];

    public void AddWindow(WindowViewModelBase window)
    {
        if (FindWindowWrapper(window) is null)
        {
            var (leftX, topY, width, height) = LoadWindowSavedPositionSize(window);

            //create new wrapper.
            var wrapper = new WindowViewModelWrapper(window, leftX, topY, width, height);

            LimitWindowPositionAndSize(wrapper);

            Logger.LogInformationEx($"added window: [{wrapper.ViewModel.GetHashCode()}]{wrapper.ViewModel.Title}");
            WindowViewModelWrappers.Add(wrapper);
        }

        MakeFrontShow(window);
    }

    public void RemoveWindow(WindowViewModelBase window)
    {
        if (FindWindowWrapper(window) is not { } wrapper)
            return;
        WindowViewModelWrappers.Remove(wrapper);
        SaveWindowSavedPositionSize(wrapper);
        Logger.LogInformationEx($"remove window: [{wrapper.ViewModel.GetHashCode()}]{wrapper.ViewModel.Title}");

        MakeFrontShow(WindowViewModelWrappers.OrderByDescending(x => x.ZIndex).FirstOrDefault()?.ViewModel);
    }

    public void MakeFrontShow(WindowViewModelBase window)
    {
        if (window is null)
            return;
        if (FindWindowWrapper(window) is not { } wrapper)
            return;
        var resortIndex = 0;
        foreach (var windowViewModel in WindowViewModelWrappers.OrderBy(x => x.ZIndex))
        {
            windowViewModel.ZIndex = resortIndex++;
            windowViewModel.IsActive = false;
        }

        wrapper.ZIndex = resortIndex * 10;
        wrapper.IsActive = true;
        Logger.LogInformationEx($"make window front: [{wrapper.ViewModel.GetHashCode()}]{wrapper.ViewModel.Title}");
    }

    private WindowViewModelWrapper FindWindowWrapper(WindowViewModelBase window)
    {
        return WindowViewModelWrappers.FirstOrDefault(x => x.ViewModel == window) ??
               MinimizedWindowViewModelWrappers.FirstOrDefault(x => x.ViewModel == window);
    }

    private void LimitWindowPositionAndSize(WindowViewModelWrapper wrapper)
    {
        if (view is not Control control)
            return;

        //limit window position&size
        wrapper.LeftX = Math.Clamp(wrapper.LeftX, 0, control.Bounds.Width);
        wrapper.TopY = Math.Clamp(wrapper.TopY, 0, control.Bounds.Height);
        wrapper.Width = Math.Clamp(wrapper.Width.Value, 0d, control.Bounds.Width - wrapper.LeftX);
        wrapper.Height = Math.Clamp(wrapper.Height.Value, 0d, control.Bounds.Height - wrapper.TopY);
    }

    public override void OnViewAfterLoaded(IView view)
    {
        base.OnViewAfterLoaded(view);
        this.view = view;
    }

    public override void OnViewBeforeUnload(IView view)
    {
        base.OnViewBeforeUnload(view);
        this.view = null;
    }

    private void SaveWindowSavedPositionSize(WindowViewModelWrapper windowWrapper)
    {
        if (windowWrapper?.ViewModel?.GetType().FullName is not {Length: > 0} key)
            return;
        windowPositionSizeSetting.WindowPositionSizeMap[key] =
            new ControlPositionSize(windowWrapper.LeftX, windowWrapper.TopY, windowWrapper.Width, windowWrapper.Height);

        settingManager.SaveSetting(windowPositionSizeSetting, WindowPositionSizeSetting.JsonTypeInfo);
    }

    private (double? leftX, double? topY, ControlSize width, ControlSize height) LoadWindowSavedPositionSize(
        WindowViewModelBase window)
    {
        if (window?.GetType().FullName is not {Length: > 0} key)
            return default;

        return !windowPositionSizeSetting.WindowPositionSizeMap.TryGetValue(key, out var windowPositionSize)
            ? default
            : (windowPositionSize.LeftX, windowPositionSize.TopY, windowPositionSize.Width, windowPositionSize.Height);
    }

    [RelayCommand]
    private void ResetWindowPositionAndSize(WindowViewModelBase window)
    {
        if (WindowViewModelWrappers.FirstOrDefault(x => x.ViewModel == window) is not { } wrapper)
            return;

        wrapper.LeftX = wrapper.ViewModel.DefaultLeftX;
        wrapper.TopY = wrapper.ViewModel.DefaultTopY;
        wrapper.Width = wrapper.ViewModel.DefaultWidth;
        wrapper.Height = wrapper.ViewModel.DefaultHeight;

        LimitWindowPositionAndSize(wrapper);
    }


    [RelayCommand]
    private void CloseWindow(WindowViewModelBase window)
    {
        RemoveWindow(window);
    }

    [RelayCommand]
    private void FocusWindow(WindowViewModelBase window)
    {
        MakeFrontShow(window);
    }

    [RelayCommand]
    private void MinimizeWindow(WindowViewModelBase window)
    {
        MakeFrontShow(window);
    }
}