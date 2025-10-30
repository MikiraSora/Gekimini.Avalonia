using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Ursa.Controls;

namespace Gekimini.Avalonia.Framework.Services
{
    public interface IShell
    {
        event EventHandler<IDocumentViewModel> ActiveDocumentChanged;
        
        bool ShowFloatingWindowsInTaskbar { get; set; }
        /*
        IMenu MainMenu { get; }
        IToolBars ToolBars { get; }
        IStatusBar StatusBar { get; }
        */

        IEnumerable<IDocumentViewModel> Documents { get; }
        IEnumerable<IToolViewModel> Tools { get; }

        void ShowTool<TTool>() where TTool : IToolViewModel;
        void ShowTool(IToolViewModel model);
        void HideTool<TTool>() where TTool : IToolViewModel;
        void HideTool(IToolViewModel model);

        Task OpenDocumentAsync(IDocumentViewModel model);
        Task CloseDocumentAsync(IDocumentViewModel document);

        void Close();
    }
}
