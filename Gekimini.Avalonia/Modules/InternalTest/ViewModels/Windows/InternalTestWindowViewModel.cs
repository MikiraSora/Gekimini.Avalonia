using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Utils.MethodExtensions;

namespace Gekimini.Avalonia.Modules.InternalTest.ViewModels.Windows;

public partial class InternalTestWindowViewModel : WindowViewModelBase
{
    public InternalTestWindowViewModel()
    {
        Title = "Internal Test Window~".ToLocalizedStringByRawText();
    }

    [ObservableProperty]
    private partial int CurrentValue { get; set; } = 50;

    [RelayCommand]
    private void CloseWindow()
    {
        TryCloseAsync(true).NoWait();
    }
}