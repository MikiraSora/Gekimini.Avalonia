using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.MainView.Views;

public partial class MainView : ViewBase
{
    private bool _dragging;
    private Point _dragStart;
    private bool _resizing;
    private Point _startPosition;
    private Size _startSize;
    private Control control;

    public MainView()
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

        control.Width = Math.Max(150, _startSize.Width + delta.X);
        control.Height = Math.Max(100, _startSize.Height + delta.Y);
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

        Canvas.SetLeft(control, _startPosition.X + delta.X);
        Canvas.SetTop(control, _startPosition.Y + delta.Y);
    }

    private void StopDragging(object sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
        PointerMoved -= DragMove;
        PointerReleased -= StopDragging;
    }
}