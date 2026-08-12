using System.Threading.Tasks;
using Gekimini.Avalonia.Modules.Settings.ViewModels;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Modules.Window.Views;
using Iciclecreek.Avalonia.WindowManager;

namespace Gekimini.Avalonia.Platforms.Services.Window;

public interface IWindowManager
{
    WindowViewBase FindExistingWindow(WindowViewModelBase windowViewModel);

    Task ShowWindowAsync(WindowViewBase windowView);
    Task<bool?> ShowDialogAsync(WindowViewBase windowView);
    Task TryCloseWindowAsync(WindowViewBase windowView, bool dialogResult);

    Task ShowWindowAsync(WindowViewModelBase windowViewModel);
    Task<bool?> ShowDialogAsync(WindowViewModelBase windowViewModel);
    Task TryCloseWindowAsync(WindowViewModelBase windowViewModelBase, bool dialogResult);
    
}