using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Framework.DragDrops.Behaviors;

public class DragDataContextOutBehavior : Behavior<Control>
{
    public const int MinimumHorizontalDragDistance = 4;
    public const int MinimumVerticalDragDistance = 4;

    private bool _draggingItem;
    private Point _mouseStartPosition;

    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        
        AssociatedObject.PointerMoved += AssociatedObjectOnPointerMoved;
        AssociatedObject.PointerPressed += AssociatedObjectOnPointerPressed;
        AssociatedObject.PointerReleased += AssociatedObjectOnPointerReleased;
    }

    private void AssociatedObjectOnPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _draggingItem = false;
    }

    private void AssociatedObjectOnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        _mouseStartPosition = e.GetPosition(e.Source as Control);
    }

    private void AssociatedObjectOnPointerMoved(object sender, PointerEventArgs e)
    {
        if (_draggingItem)
            return;

        // Get the current mouse position
        var mousePosition = e.GetPosition(null);
        var diff = _mouseStartPosition - mousePosition;

        var isPressed = e.Properties.IsLeftButtonPressed;

        if (isPressed &&
            (Math.Abs(diff.X) > MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > MinimumVerticalDragDistance))
        {
            var dataContext = (e.Source as Control)?.DataContext;

            if (dataContext == null)
                return;

            (App.Current as App).ServiceProvider.GetService<IDragDropManager>()
                .StartDragDropEvent(e, dataContext, DragDropEffects.Move);

            _draggingItem = true;
        }
    }

    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();

        AssociatedObject.PointerMoved -= AssociatedObjectOnPointerMoved;
        AssociatedObject.PointerPressed -= AssociatedObjectOnPointerPressed;
        AssociatedObject.PointerReleased -= AssociatedObjectOnPointerReleased;
    }
}