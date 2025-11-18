using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Framework.DragDrops;
using Gekimini.Avalonia.Modules.Toolbox.ViewModels;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.Toolbox.Views;

public partial class ToolboxView : ViewBase
{
    public const int MinimumHorizontalDragDistance = 4;
    public const int MinimumVerticalDragDistance = 4;

    private readonly IDragDropManager _dragDropManager;
    private bool _draggingItem;
    private Point _mouseStartPosition;

    public ToolboxView(IDragDropManager dragDropManager)
    {
        _dragDropManager = dragDropManager;
        InitializeComponent();
    }

    private void OnListBoxPreviewMouseLeftButtonDown(object sender, PointerPressedEventArgs e)
    {
        var listBoxItem = (e.Source as Control).FindAncestorOfType<ListBoxItem>(true);
        _draggingItem = listBoxItem != null;

        _mouseStartPosition = e.GetPosition(listBox);
    }

    private void OnListBoxMouseMove(object sender, PointerEventArgs e)
    {
        if (!_draggingItem)
            return;

        // Get the current mouse position
        var mousePosition = e.GetPosition(null);
        Vector diff = _mouseStartPosition - mousePosition;

        var isPressed = e.GetCurrentPoint(listBox).Properties.IsLeftButtonPressed;

        if (isPressed &&
            (Math.Abs(diff.X) > MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > MinimumVerticalDragDistance))
        {
            var listBoxItem = (e.Source as Control).FindAncestorOfType<ListBoxItem>(true);

            if (listBoxItem == null)
                return;

            var itemModel = ((ToolboxItemViewModel) listBoxItem.DataContext).Model;

            _dragDropManager.StartDragDropEvent(e, itemModel, DragDropEffects.Move);
        }
    }
}