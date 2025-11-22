using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dock.Model.Core;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Modules.MainMenu;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Modules.ToolBars;

namespace Gekimini.Avalonia.Modules.Shell;

public interface IShell
{
    bool ShowFloatingWindowsInTaskbar { get; set; }
    IStatusBar StatusBar { get; }

    IDockable ActiveDockable { get; }
    IDocumentViewModel ActiveDocument { get; }
    IMenu MainMenu { get; }
    IToolBars ToolBars { get; }

    IEnumerable<IDocumentViewModel> Documents { get; }
    IEnumerable<IToolViewModel> Tools { get; }
    event EventHandler<IDocumentViewModel> ActiveDocumentChanged;

    void ShowTool<TTool>() where TTool : IToolViewModel;
    void ShowTool(IToolViewModel model);
    void HideTool<TTool>() where TTool : IToolViewModel;
    void HideTool(IToolViewModel model);

    Task OpenDocumentAsync(IDocumentViewModel model);
    Task CloseDocumentAsync(IDocumentViewModel document);
}