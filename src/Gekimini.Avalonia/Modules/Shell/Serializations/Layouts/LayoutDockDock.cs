using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutDockDock : LayoutDock
{
    public bool LastChildFill { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);

        if (f is IDockDock from)
        {
            LastChildFill = from.LastChildFill;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IDockDock to)
        {
            to.LastChildFill = LastChildFill;
        }
    }
}