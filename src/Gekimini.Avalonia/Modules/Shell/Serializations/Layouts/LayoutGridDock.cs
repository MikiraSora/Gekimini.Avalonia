using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutGridDock : LayoutDock
{
    public string ColumnDefinitions { get; set; }
    public string RowDefinitions { get; set; }
    
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IGridDock from)
        {
            ColumnDefinitions = from.ColumnDefinitions;
            RowDefinitions = from.RowDefinitions;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IGridDock to)
        {
            to.ColumnDefinitions = ColumnDefinitions;
            to.RowDefinitions = RowDefinitions;
        }
    }
}