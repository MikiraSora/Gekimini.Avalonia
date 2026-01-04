using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Gekimini.Avalonia.Views;
using Iciclecreek.Avalonia.WindowManager;

namespace Gekimini.Avalonia.Modules.Window.Views;

public class WindowViewBase : ManagedWindow, IView
{
    private bool isAdjusting;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        //there is a bug in PART_SystemMenu, just hide it
        if (e.NameScope.Find<Control>("PART_SystemMenu") is { } systemMenu)
            systemMenu.IsVisible = false;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        switch (change.Property.Name)
        {
            case nameof(Width):
            case nameof(Height):
                AdjustSize(WindowsPanel?.Bounds);
                break;
            case nameof(Position):
                AdjustPosition(WindowsPanel?.Bounds);
                break;
        }
    }

    public void AdjustWindowPositionAndSize(WindowsPanel windowsPanel)
    {
        var actualWindowPanel = windowsPanel ?? WindowsPanel;
        if (actualWindowPanel is null)
            return;

        var bounds = actualWindowPanel.Bounds;

        AdjustPosition(bounds);
        AdjustSize(bounds);
    }

    private void AdjustSize(Rect? b)
    {
        if (b is not { } bounds)
            return;

        if (isAdjusting)
            return;
        isAdjusting = true;

        const int MIN_LENGTH = 50;
        var width = Math.Clamp(Width, MIN_LENGTH, bounds.Width - Position.X);
        var height = Math.Clamp(Height, MIN_LENGTH, bounds.Height - Position.Y);

        const float TOLERANCE = 0.000001f;

        if (Math.Abs(Width - width) > TOLERANCE)
            Width = width;

        if (Math.Abs(Height - height) > TOLERANCE)
            Height = height;

        isAdjusting = false;
    }

    private void AdjustPosition(Rect? b)
    {
        if (b is not { } bounds)
            return;

        if (isAdjusting)
            return;
        isAdjusting = true;

        var pos = new PixelPoint((int) Math.Clamp(Position.X, 0, Math.Max(bounds.Width - Width, 1)),
            (int) Math.Clamp(Position.Y, 0, Math.Max(bounds.Height - Height, 1)));

        if (pos != Position)
            Position = pos;

        isAdjusting = false;
    }
}