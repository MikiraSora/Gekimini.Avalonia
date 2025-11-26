using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.EmbeddedWindows.Views;

public partial class EmbeddedWindowView : ViewBase
{
    private bool _dragging;
    private Point _dragStart;
    private bool _resizing;
    private Point _startPosition;
    private Size _startSize;
    private Control control;

    public EmbeddedWindowView()
    {
        InitializeComponent();
    }

    private void InputElement_Resize(object sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        control = (sender as Control).FindAncestorOfType<ContentPresenter>();
        if (control is null)
            return;

        _resizing = true;
        _dragStart = e.GetPosition(null);
        _startSize = control.Bounds.Size;

        PointerMoved += ResizeMove;
        PointerReleased += StopResizing;
    }

    private void ResizeMove(object sender, PointerEventArgs e)
    {
        if (!_resizing) return;

        var pos = e.GetPosition(null);
        var delta = pos - _dragStart;

        var width = _startSize.Width + delta.X;
        width = Math.Clamp(width, 50, Bounds.Width - Canvas.GetLeft(control));

        var height = _startSize.Height + delta.Y;
        height = Math.Clamp(height, 50, Bounds.Height - Canvas.GetTop(control));

        control.Width = width;
        control.Height = height;
    }

    private void StopResizing(object sender, PointerReleasedEventArgs e)
    {
        _resizing = false;
        PointerMoved -= ResizeMove;
        PointerReleased -= StopResizing;
    }

    private void InputElement_Drag(object sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        control = (sender as Control).FindAncestorOfType<ContentPresenter>();
        if (control is null)
            return;

        _dragging = true;
        _dragStart = e.GetPosition(null);
        _startPosition = new Point(Canvas.GetLeft(control), Canvas.GetTop(control));

        PointerMoved += DragMove;
        PointerReleased += StopDragging;
    }

    private void DragMove(object sender, PointerEventArgs e)
    {
        if (!_dragging) return;

        var pos = e.GetPosition(null);
        var delta = pos - _dragStart;

        var left = _startPosition.X + delta.X;
        left = Math.Clamp(left, 0, Bounds.Width - control.Bounds.Width);

        var top = _startPosition.Y + delta.Y;
        top = Math.Clamp(top, 0, Bounds.Height - control.Bounds.Height);

        Canvas.SetLeft(control, left);
        Canvas.SetTop(control, top);
    }

    private void StopDragging(object sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
        PointerMoved -= DragMove;
        PointerReleased -= StopDragging;
    }

    private void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        control = (sender as Control).FindAncestorOfType<ContentPresenter>();
        if (control is null)
            return;

        var width = control.Bounds.Width;
        width = Math.Clamp(width, 50, Bounds.Width - Canvas.GetLeft(control));
        control.Width = width;

        var height = control.Bounds.Height;
        height = Math.Clamp(height, 50, Bounds.Height - Canvas.GetTop(control));
        control.Height = height;
    }
}