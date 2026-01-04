using System.Collections.Generic;
using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutStackDock : LayoutDock
{
    public Orientation Orientation { get; set; }
    public double Spacing { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IStackDock from)
        {
            Orientation = from.Orientation;
            Spacing = from.Spacing;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IStackDock to)
        {
            to.Orientation = Orientation;
            to.Spacing = Spacing;
        }
    }
}