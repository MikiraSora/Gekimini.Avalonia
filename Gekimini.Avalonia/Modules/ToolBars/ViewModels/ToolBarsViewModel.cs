using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.ToolBar.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Modules.ToolBars.Views;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.ToolBars.ViewModels;

[RegisterSingleton<IToolBars>]
public partial class ToolBarsViewModel : ViewModelBase, IToolBars
{
    private readonly IToolBarBuilder _toolBarBuilder;

    [ObservableProperty]
    private bool visible = true;

    public ToolBarsViewModel(IToolBarBuilder toolBarBuilder)
    {
        _toolBarBuilder = toolBarBuilder;
    }

    public ObservableCollection<IToolBar> Items { get; } = new();

    public override void OnViewAfterLoaded(IView view)
    {
        base.OnViewAfterLoaded(view);

        _toolBarBuilder.BuildToolBars(this);

        // TODO: Ideally, the ToolBarTray control would expose ToolBars
        // as a dependency property. We could use an attached property
        // to workaround this. But for now, toolbars need to be
        // created prior to the following code being run.
        if (((IToolBarsView) view).ToolBarTray is not { } toolBarTray)
            return;

        foreach (var toolBar in Items)
            toolBarTray.ToolBars.Add(new ToolBar
            {
                ItemsSource = toolBar,
                Orientation = Orientation.Horizontal,
            });
    }
}