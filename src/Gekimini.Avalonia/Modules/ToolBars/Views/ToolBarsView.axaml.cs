using Avalonia.Controls.ToolBar.Controls;
using Avalonia.Markup.Xaml;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.ToolBars.Views;

public partial class ToolBarsView : ViewBase, IToolBarsView
{
    public ToolBarsView()
    {
        InitializeComponent();
    }

    public ToolBarTray ToolBarTray => toolBarTray;
}