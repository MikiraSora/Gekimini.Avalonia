using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Avalonia.Controls.ToolBar.Controls;

public partial class ToolBar
{
    #region Orientation Property

    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<ToolBar, Orientation>(nameof(Orientation), inherits: true,
            coerce: CoerceOrientation);

    static Orientation CoerceOrientation(AvaloniaObject obj, Orientation value)
    {
        var toolBarTray = ((ToolBar)obj).ToolBarTray;
        return toolBarTray != null ? toolBarTray.Orientation : value;
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    #endregion

    #region Band Property

    public static readonly StyledProperty<int> BandProperty =
        AvaloniaProperty.Register<ToolBar, int>(nameof(Band));

    public int Band
    {
        get => GetValue(BandProperty);
        set => SetValue(BandProperty, value);
    }

    #endregion

    #region BandIndex Property

    public static readonly StyledProperty<int> BandIndexProperty =
        AvaloniaProperty.Register<ToolBar, int>(nameof(BandIndex));

    public int BandIndex
    {
        get => GetValue(BandIndexProperty);
        set => SetValue(BandIndexProperty, value);
    }

    #endregion

    #region IsOverflowOpen Property

    public static readonly StyledProperty<bool> IsOverflowOpenProperty =
        AvaloniaProperty.Register<ToolBar, bool>(nameof(IsOverflowOpen),
            defaultBindingMode: BindingMode.TwoWay, coerce: CoerceIsOverflowOpen);

    public bool IsOverflowOpen
    {
        get => GetValue(IsOverflowOpenProperty);
        set => SetValue(IsOverflowOpenProperty, value);
    }

    private static bool CoerceIsOverflowOpen(AvaloniaObject obj, bool value)
    {
        if (value)
        {
            var toolBar = (ToolBar)obj;
            if (!toolBar.IsLoaded)
            {
                toolBar.RegisterToOpenOnLoad();
                return false;
            }
        }

        return value;
    }

    private void RegisterToOpenOnLoad() => Loaded += OpenOnLoad;

    private void OpenOnLoad(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() => CoerceValue(IsOverflowOpenProperty), DispatcherPriority.Input);
    }

    #endregion

    #region HasOverflowItems Property

    public static readonly StyledProperty<bool> HasOverflowItemsProperty =
        AvaloniaProperty.Register<ToolBar, bool>(nameof(HasOverflowItems));

    public bool HasOverflowItems
    {
        get => GetValue(HasOverflowItemsProperty);
        set => SetValue(HasOverflowItemsProperty, value);
    }

    #endregion

    #region IsOverflowItem Property

    public static readonly StyledProperty<bool> IsOverflowItemProperty =
        AvaloniaProperty.Register<ToolBar, bool>(nameof(IsOverflowItem), inherits: true);

    public bool IsOverflowItem
    {
        get => GetValue(IsOverflowItemProperty);
        set => SetValue(IsOverflowItemProperty, value);
    }

    public static void SetIsOverflowItem(Control control, bool value) =>
        control.SetValue(IsOverflowItemProperty, value);

    public static bool GetIsOverflowItem(Control control) => control.GetValue(IsOverflowItemProperty);

    #endregion

    #region OverflowMode Property

    public static readonly StyledProperty<OverflowMode> OverflowModeProperty =
        AvaloniaProperty.Register<ToolBar, OverflowMode>(nameof(OverflowMode),
            validate: IsValidOverflowMode);

    public OverflowMode OverflowMode
    {
        get => GetValue(OverflowModeProperty);
        set => SetValue(OverflowModeProperty, value);
    }

    private static void OnOverflowModeChanged(ToolBar toolBar, AvaloniaPropertyChangedEventArgs e) =>
        toolBar.InvalidateLayout();

    private static bool IsValidOverflowMode(OverflowMode value) =>
        value is OverflowMode.AsNeeded or OverflowMode.Always or OverflowMode.Never;

    #endregion

    #region MinVisibleItemsCount Property

    public static readonly StyledProperty<uint> MinVisibleItemsCountProperty =
        AvaloniaProperty.Register<ToolBar, uint>(nameof(MinVisibleItemsCountProperty), defaultValue: 0);

    public uint MinVisibleItemsCount
    {
        get => GetValue(MinVisibleItemsCountProperty);
        set => SetValue(MinVisibleItemsCountProperty, value);
    }

    private static void OnMinVisibleItemsCountChanged(ToolBar toolBar, AvaloniaPropertyChangedEventArgs e) =>
        toolBar.InvalidateLayout();

    #endregion

    static ToolBar()
    {
        OverflowModeProperty.Changed.AddClassHandler<ToolBar>(OnOverflowModeChanged);
        MinVisibleItemsCountProperty.Changed.AddClassHandler<ToolBar>(OnMinVisibleItemsCountChanged);
        IsTabStopProperty.OverrideMetadata<ToolBar>(new StyledPropertyMetadata<bool>(false));
        FocusableProperty.OverrideDefaultValue<ToolBar>(true);
        KeyboardNavigation.TabNavigationProperty.OverrideMetadata<ToolBar>(
            new StyledPropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Cycle));
        Button.ClickEvent.AddClassHandler<ToolBar>((x, e) => x.OnClick(e));
    }
}
