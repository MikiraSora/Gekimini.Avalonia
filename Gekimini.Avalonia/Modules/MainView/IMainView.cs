using System.Collections.ObjectModel;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Modules.MainView;

public interface IMainView
{
    IShell Shell { get; }
}