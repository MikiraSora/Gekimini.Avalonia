using System.Collections.Generic;
using Avalonia.Controls;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar.ViewModels;

[RegisterSingleton<IStatusBar>]
public class StatusBarViewModel : ViewModelBase, IStatusBar
{
    private readonly List<StatusBarItemViewModel> items = [];
    private IStatusBarView statusBarView;

    public StatusBarViewModel()
    {
        items.Add(new StatusBarItemViewModel(string.Empty, GridLength.Auto));
        items.Add(new StatusBarItemViewModel(string.Empty, GridLength.Auto));
        items.Add(new StatusBarItemViewModel(string.Empty, GridLength.Auto));
    }

    public StatusBarManager StatusBarManager => statusBarView.StatusBarManager;
    public IReadOnlyList<StatusBarItemViewModel> Items => items;

    public override void OnViewAfterLoaded(IView view)
    {
        base.OnViewAfterLoaded(view);
        statusBarView = view as IStatusBarView;

        if (statusBarView == null)
            return;

        var barItem = StatusBarManager.CreateStatusBarItem("__left");
        BuildBarItemBinding(barItem, items[0]);
        barItem.Show();

        barItem = StatusBarManager.CreateStatusBarItem("__centerright", StatusBarAlignment.Right);
        BuildBarItemBinding(barItem, items[1]);
        barItem.Show();

        barItem = StatusBarManager.CreateStatusBarItem("__right", StatusBarAlignment.Right, 1);
        BuildBarItemBinding(barItem, items[2]);
        barItem.Show();
    }

    private void BuildBarItemBinding(StatusBarItem barItem, StatusBarItemViewModel barItemViewModel)
    {
        barItem.Text = barItemViewModel.Message;

        barItemViewModel.PropertyChanged += (sender, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(StatusBarItemViewModel.Message):
                    barItem.Text = barItemViewModel.Message;
                    break;
                case nameof(StatusBarItemViewModel.Width):
                    //todo: not support
                    break;
            }
        };
    }

    public override void OnViewBeforeUnload(IView view)
    {
        base.OnViewBeforeUnload(view);
        statusBarView = null;
    }
}