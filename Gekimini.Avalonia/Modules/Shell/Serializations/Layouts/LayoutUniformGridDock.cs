using System.Collections.Generic;
using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutUniformGridDock : LayoutDock
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IUniformGridDock from)
        {
            Rows = from.Rows;
            Columns = from.Columns;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IUniformGridDock to)
        {
            to.Rows = Rows;
            to.Columns = Columns;
        }
    }
}