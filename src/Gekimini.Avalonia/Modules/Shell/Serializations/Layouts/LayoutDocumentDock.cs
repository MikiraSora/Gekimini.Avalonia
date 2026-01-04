using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutDocumentDock : LayoutDock
{
    public bool CanCreateDocument { get; set; }
    //public ICommand CreateDocument { get; set; }
    public bool EnableWindowDrag { get; set; }
    public DocumentTabLayout TabsLayout { get; set; }
    
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IDocumentDock from)
        {
            CanCreateDocument = from.CanCreateDocument;
            EnableWindowDrag = from.EnableWindowDrag;
            TabsLayout = from.TabsLayout;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IDocumentDock to)
        {
            to.CanCreateDocument = CanCreateDocument;
            to.EnableWindowDrag = EnableWindowDrag;
            to.TabsLayout = TabsLayout;
        }
    }
}