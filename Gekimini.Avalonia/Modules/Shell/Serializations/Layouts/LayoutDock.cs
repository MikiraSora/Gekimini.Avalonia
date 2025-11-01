using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutDock : LayoutDockable
{
    public IList<LayoutDockable> VisibleDockables { get; set; } = new List<LayoutDockable>();

    //public IDockable ActiveDockable { get; set; }
    //public IDockable DefaultDockable { get; set; }
    //public IDockable FocusedDockable { get; set; }
    public bool IsActive { get; set; }
    public int OpenedDockablesCount { get; set; }

    public bool CanCloseLastDockable { get; set; }

    //public bool CanGoBack { get; }
    //public bool CanGoForward { get; }
    //public ICommand GoBack { get; }
    // ICommand GoForward { get; }
    //public ICommand Navigate { get; }
    //public ICommand Close { get; }
    public bool EnableGlobalDocking { get; set; }

    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);

        if (f is IDock from)
        {
            IsActive = from.IsActive;
            OpenedDockablesCount = from.OpenedDockablesCount;
            CanCloseLastDockable = from.CanCloseLastDockable;
            EnableGlobalDocking = from.EnableGlobalDocking;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IDock to)
        {
            to.IsActive = IsActive;
            to.OpenedDockablesCount = OpenedDockablesCount;
            to.CanCloseLastDockable = CanCloseLastDockable;
            to.EnableGlobalDocking = EnableGlobalDocking;
        }
    }
}