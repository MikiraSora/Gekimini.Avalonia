using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Gekimini.Avalonia.Modules.ToolBars.Controls;

public sealed class AdaptiveToolBar : global::Avalonia.Controls.ToolBar.Controls.ToolBar
{
    private Control overflowButton;
    private Border mainPanelBorder;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        overflowButton = e.NameScope.Find<Control>("PART_OverflowButton");
        mainPanelBorder = e.NameScope.Find<Border>("MainPanelBorder");
        UpdateOverflowChrome();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == HasOverflowItemsProperty || change.Property == OrientationProperty)
            UpdateOverflowChrome();
    }

    private void UpdateOverflowChrome()
    {
        if (overflowButton is not null)
            overflowButton.IsVisible = HasOverflowItems;

        if (mainPanelBorder is not null)
        {
            mainPanelBorder.Margin = HasOverflowItems
                ? Orientation == global::Avalonia.Layout.Orientation.Horizontal
                    ? new Thickness(0, 0, 11, 0)
                    : new Thickness(0, 0, 0, 11)
                : new Thickness(0);
        }
    }
}
