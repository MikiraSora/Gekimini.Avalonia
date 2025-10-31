using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutProportionalDock : LayoutDock
{
    public Orientation Orientation { get; set; }
    
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IProportionalDock from)
        {
            Orientation = from.Orientation;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IProportionalDock to)
        {
            to.Orientation = Orientation;
        }
    }
}