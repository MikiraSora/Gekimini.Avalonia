using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;

namespace Gekimini.Avalonia.Modules.MainMenu.Controls;

public class MenuEx : Menu
{
    private const double CollapsedPointerTargetHeight = 2;
    private bool isAutoHideCollapsed;
    private bool pointerKeepsMenuVisible;
    private TopLevel attachedTopLevel;

    public static readonly StyledProperty<bool> AutoHideProperty =
        AvaloniaProperty.Register<MenuEx, bool>(nameof(AutoHide));

    public static readonly DirectProperty<MenuEx, bool> IsAutoHideCollapsedProperty =
        AvaloniaProperty.RegisterDirect<MenuEx, bool>(
            nameof(IsAutoHideCollapsed),
            menu => menu.IsAutoHideCollapsed);

    public bool AutoHide
    {
        get => GetValue(AutoHideProperty);
        set => SetValue(AutoHideProperty, value);
    }

    public bool IsAutoHideCollapsed => isAutoHideCollapsed;

    protected override Type StyleKeyOverride => typeof(Menu);

    protected override Control CreateContainerForItemOverride(object item, int index, object recycleKey)
    {
        if (item is SeparatorItemViewModel)
            return new Separator();

        return new MenuItemEx();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        attachedTopLevel = TopLevel.GetTopLevel(this);
        attachedTopLevel?.AddHandler(
            InputElement.KeyDownEvent,
            OnTopLevelKeyDown,
            RoutingStrategies.Tunnel,
            handledEventsToo: true);
        UpdateAutoHideState();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        attachedTopLevel?.RemoveHandler(InputElement.KeyDownEvent, OnTopLevelKeyDown);
        attachedTopLevel = null;
        pointerKeepsMenuVisible = false;

        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        pointerKeepsMenuVisible = true;
        UpdateAutoHideState();

        base.OnPointerEntered(e);
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        pointerKeepsMenuVisible = false;
        UpdateAutoHideState();

        base.OnPointerExited(e);
    }

    protected override void OnSubmenuOpened(RoutedEventArgs e)
    {
        base.OnSubmenuOpened(e);
        UpdateAutoHideState();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == AutoHideProperty ||
            change.Property == IsKeyboardFocusWithinProperty ||
            change.Property == IsOpenProperty)
        {
            UpdateAutoHideState();
        }
    }

    private void OnTopLevelKeyDown(object sender, KeyEventArgs e)
    {
        if (!AutoHide || !IsMenuActivationKey(e))
            return;

        pointerKeepsMenuVisible = false;
        SetCollapsed(false);
        UpdateLayout();

        var firstItem = ContainerFromIndex(0) as MenuItem ??
                        this.GetLogicalDescendants().OfType<MenuItem>().FirstOrDefault();
        firstItem?.Focus(NavigationMethod.Tab, e.KeyModifiers);
        e.Handled = true;
    }

    private void UpdateAutoHideState()
    {
        var shouldCollapse = AutoHide &&
                             !pointerKeepsMenuVisible &&
                             !IsKeyboardFocusWithin &&
                             !IsOpen &&
                             !HasOpenSubmenu();
        SetCollapsed(shouldCollapse);
    }

    private bool HasOpenSubmenu()
    {
        return this.GetLogicalDescendants()
            .OfType<MenuItem>()
            .Any(static item => item.IsSubMenuOpen);
    }

    private void SetCollapsed(bool collapsed)
    {
        if (isAutoHideCollapsed == collapsed)
            return;

        SetAndRaise(IsAutoHideCollapsedProperty, ref isAutoHideCollapsed, collapsed);
        SetCurrentValue(MaxHeightProperty, collapsed ? CollapsedPointerTargetHeight : double.PositiveInfinity);
        SetCurrentValue(OpacityProperty, collapsed ? 0 : 1);
        SetCurrentValue(ClipToBoundsProperty, collapsed);
    }

    private static bool IsMenuActivationKey(KeyEventArgs e)
    {
        if (e.Key is Key.LeftAlt or Key.RightAlt)
            return (e.KeyModifiers & ~KeyModifiers.Alt) == KeyModifiers.None;

        return e.Key == Key.F10 && (e.KeyModifiers & ~KeyModifiers.Alt) == KeyModifiers.None;
    }
}
