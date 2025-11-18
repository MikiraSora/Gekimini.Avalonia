using System.Threading.Tasks;
using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Platforms.Services.Window;

public interface IWindowManager
{
    Task ShowDialogAsync(WindowViewModelBase windowViewModel);
    Task TryCloseAsync(WindowViewModelBase windowViewModelBase, bool dialogResult);
}