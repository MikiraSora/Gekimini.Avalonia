using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Models;
using Gekimini.Avalonia.Platforms.Services.Window;
using Gekimini.Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Window.ViewModels;

public partial class WindowViewModelBase : ViewModelBase
{
    protected Task TryCloseAsync(bool dialogResult)
    {
        return (Application.Current as App)?.ServiceProvider?.GetService<IWindowManager>()
            .TryCloseWindowAsync(this, dialogResult);
    }
}