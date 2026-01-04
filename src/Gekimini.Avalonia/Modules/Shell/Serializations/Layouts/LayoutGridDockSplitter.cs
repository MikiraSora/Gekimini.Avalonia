using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutGridDockSplitter : LayoutSplitter
{
    public GridResizeDirection ResizeDirection { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);

        if (f is IGridDockSplitter from)
        {
            ResizeDirection = from.ResizeDirection;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IGridDockSplitter to)
        {
            to.ResizeDirection = ResizeDirection;
        }
    }
}