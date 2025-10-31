using System.Collections.Generic;
using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutWrapDock : LayoutDock
{
    public Orientation Orientation { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IWrapDock from)
        {
            Orientation = from.Orientation;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IWrapDock to)
        {
            to.Orientation = Orientation;
        }
    }
}