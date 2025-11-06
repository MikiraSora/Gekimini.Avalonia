using System.Collections.ObjectModel;
using Avalonia.Controls;
using Gekimini.Avalonia.Modules.StatusBar.ViewModels;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar;

public interface IStatusBar
{
    StatusBarManager StatusBarManager { get; }
}