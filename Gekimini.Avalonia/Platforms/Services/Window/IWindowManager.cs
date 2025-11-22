using System.Threading.Tasks;
using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Platforms.Services.Window;

public interface IWindowManager
{
    Task ShowWindowAsync(WindowViewModelBase windowViewModel);
    Task TryCloseWindowAsync(WindowViewModelBase windowViewModelBase, bool dialogResult);
}