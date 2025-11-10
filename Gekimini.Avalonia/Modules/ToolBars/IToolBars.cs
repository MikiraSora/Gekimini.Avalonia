using System.Collections.ObjectModel;

namespace Gekimini.Avalonia.Modules.ToolBars;

public interface IToolBars
{
    ObservableCollection<IToolBar> Items {get;}
    bool Visible { get; set; }
}