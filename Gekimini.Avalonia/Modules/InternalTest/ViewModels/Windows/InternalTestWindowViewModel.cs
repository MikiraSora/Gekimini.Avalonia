using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Modules.InternalTest.ViewModels.Windows;

public partial class InternalTestWindowViewModel : WindowViewModelBase
{
    [ObservableProperty]
    private partial int CurrentValue { get; set; } = 50;

    public InternalTestWindowViewModel()
    {
        Title = "Internal Test Window~";
    }

    [RelayCommand]
    private void CloseWindow()
    {
        TryCloseAsync(true).NoWait();
    }
}