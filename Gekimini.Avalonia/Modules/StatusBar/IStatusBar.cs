using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData.Binding;
using Gekimini.Avalonia.Modules.StatusBar.ViewModels;
using StatusBar.Avalonia;

namespace Gekimini.Avalonia.Modules.StatusBar;

//todo 现在的StatusBar不够健壮多功能, 后面考虑和StatusBar扩展更多功能
public interface IStatusBar
{
    StatusBarManager StatusBarManager { get; }
    
    IReadOnlyList<StatusBarItemViewModel> Items { get; }
}