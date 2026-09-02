using System;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Avalonia.Controls.ToolBar.Controls;

[TemplatePart(ElementToolBarPanel, typeof(ToolBarPanel), IsRequired = true)]
[TemplatePart(ElementToolBarOverflowPanel, typeof(ToolBarOverflowPanel), IsRequired = true)]
[TemplatePart(ElementOverflowButton, typeof(ToggleButton), IsRequired = true)]
[TemplatePart(ElementToolBarPopup, typeof(Popup), IsRequired = true)]
public partial class ToolBar : HeaderedItemsControl
{
    protected override Type StyleKeyOverride => typeof(ToolBar);

    private const string PcDropdownOpen = ":dropdownopen";
    private const string ElementToolBarOverflowPanel = "PART_ToolBarOverflowPanel";
    private const string ElementToolBarPanel = "PART_ToolBarPanel";
    private const string ElementOverflowButton = "PART_OverflowButton";
    private const string ElementToolBarPopup = "PART_OverflowPopup";

    private ToggleButton? _overflowButton;
    private Popup? _popup;

    public ToolBar()
    {
        Items.CollectionChanged += OnItemsChanged;
    }

    internal ToolBarPanel? ToolBarPanel { get; private set; }
    internal ToolBarOverflowPanel? ToolBarOverflowPanel { get; private set; }
    internal double MinLength { get; private set; }
    internal double MaxLength { get; private set; }
    private ToolBarTray? ToolBarTray => Parent as ToolBarTray;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == BandProperty || change.Property == BandIndexProperty)
        {
            if (Parent is not Layoutable visualParent)
                return;

            visualParent.InvalidateMeasure();
        }
        else if (change.Property == IsOverflowOpenProperty)
        {
            PseudoClasses.Set(PcDropdownOpen, change.GetNewValue<bool>());
        }

        base.OnPropertyChanged(change);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        ToolBarPanel = e.NameScope.Find<ToolBarPanel>(ElementToolBarPanel);
        ToolBarOverflowPanel = e.NameScope.Find<ToolBarOverflowPanel>(ElementToolBarOverflowPanel);
        _overflowButton = e.NameScope.Find<ToggleButton>(ElementOverflowButton);
        _popup = e.NameScope.Find<Popup>(ElementToolBarPopup);
    }

    protected override void OnTemplateChanged(AvaloniaPropertyChangedEventArgs e)
    {
        ToolBarPanel = null;
        ToolBarOverflowPanel = null;
        base.OnTemplateChanged(e);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var desiredSize = base.MeasureOverride(constraint);
        var toolBarPanel = ToolBarPanel;
        if (toolBarPanel != null)
        {
            var margin = toolBarPanel.Margin;
            var extraLength = toolBarPanel.Orientation == Orientation.Horizontal
                ? Math.Max(0.0, desiredSize.Width - toolBarPanel.DesiredSize.Width + margin.Left + margin.Right)
                : Math.Max(0.0, desiredSize.Height - toolBarPanel.DesiredSize.Height + margin.Top + margin.Bottom);
            MinLength = toolBarPanel.MinLength + extraLength;
            MaxLength = toolBarPanel.MaxLength + extraLength;
        }

        return desiredSize;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e is { Handled: false, Source: Visual source } && _popup?.IsInsidePopup(source) == true)
        {
            e.Handled = true;
            return;
        }

        if (IsOverflowOpen)
        {
            SetCurrentValue(IsOverflowOpenProperty, false);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (e is { Handled: false, Source: Visual source } &&
            _popup?.IsInsidePopup(source) == true &&
            source.FindAncestorOfType<SelectingItemsControl>() == null &&
            source.FindAncestorOfType<AutoCompleteBox>() == null)
        {
            SetCurrentValue(IsOverflowOpenProperty, false);
            e.Handled = true;
        }

        base.OnPointerReleased(e);
    }

    internal void AddLogicalChild(Control control)
    {
        if (!LogicalChildren.Contains(control))
            LogicalChildren.Add(control);
    }

    internal void RemoveLogicalChild(Control control) => LogicalChildren.Remove(control);

    private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e) => InvalidateLayout();

    private void InvalidateLayout()
    {
        MinLength = 0.0;
        MaxLength = 0.0;
        InvalidateMeasure();
        ToolBarPanel?.InvalidateMeasure();
    }

    private void OnClick(RoutedEventArgs e)
    {
        if (Equals(e.Source, _overflowButton))
            return;

        if (IsOverflowOpen && e.Source is Button button &&
            Equals(button.FindLogicalAncestorOfType<ToolBar>(), this))
        {
            SetCurrentValue(IsOverflowOpenProperty, false);
        }
    }
}
