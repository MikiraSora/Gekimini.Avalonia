using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Modules.EmbeddedWindows.ViewModels;

public partial class WindowViewModelWrapper : ObservableObject
{
    private ControlSize height;
    private double? leftX;
    private double? topY;
    private ControlSize width;

    public WindowViewModelWrapper(WindowViewModelBase viewModel, double? leftX, double? topY, ControlSize width,
        ControlSize height)
    {
        ViewModel = viewModel;

        this.leftX = leftX;
        this.topY = topY;
        this.width = width;
        this.height = height;
    }

    public WindowViewModelBase ViewModel { get; init; }

    [ObservableProperty]
    public partial int ZIndex { get; set; }

    [ObservableProperty]
    public partial bool IsActive { get; set; }

    public double LeftX
    {
        get => leftX ?? ViewModel.DefaultLeftX;
        set
        {
            leftX = value;
            OnPropertyChanged();
        }
    }

    public double TopY
    {
        get => topY ?? ViewModel.DefaultTopY;
        set
        {
            topY = value;
            OnPropertyChanged();
        }
    }

    public ControlSize Width
    {
        get => width ?? ViewModel.DefaultWidth;
        set
        {
            width = value;
            OnPropertyChanged();
        }
    }

    public ControlSize Height
    {
        get => height ?? ViewModel.DefaultHeight;
        set
        {
            height = value;
            OnPropertyChanged();
        }
    }
}