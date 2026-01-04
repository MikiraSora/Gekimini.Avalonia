using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.StatusBar.ViewModels;

public partial class StatusBarItemViewModel : ViewModelBase
{
    public StatusBarItemViewModel(string message, GridLength width)
    {
        Message = message;
        Width = width;
    }
    
    [ObservableProperty]
    public partial string Message { get; set; }

    [ObservableProperty]
    public partial GridLength Width { get; set; }
}