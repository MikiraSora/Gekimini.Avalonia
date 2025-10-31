using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutProportionalDockSplitter : LayoutSplitter
{
    public bool CanResize { get; set; }
    public bool ResizePreview { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);

        if (f is IProportionalDockSplitter from)
        {
            CanResize = from.CanResize;
            ResizePreview = from.ResizePreview;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IProportionalDockSplitter to)
        {
            to.CanResize = CanResize;
            to.ResizePreview = ResizePreview;
        }
    }
}