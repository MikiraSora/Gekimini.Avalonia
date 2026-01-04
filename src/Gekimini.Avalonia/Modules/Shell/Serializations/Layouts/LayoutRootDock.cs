using System.Collections.Generic;
using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutRootDock : LayoutDock
{
    public bool IsFocusableRoot { get; set; }
    //public IList<IDockable> HiddenDockables { get; set; }
    //public IList<IDockable> LeftPinnedDockables { get; set; }
    //public IList<IDockable> RightPinnedDockables { get; set; }
    //public IList<IDockable> TopPinnedDockables { get; set; }
    //public IList<IDockable> BottomPinnedDockables { get; set; }
    //public IToolDock PinnedDock { get; set; }
    public LayoutDockWindow Window { get; set; }
    public IList<LayoutDockWindow> Windows { get; set; } = new List<LayoutDockWindow>();
    //public ICommand ShowWindows { get; }
    //public ICommand ExitWindows { get; }
    public bool EnableAdaptiveGlobalDockTargets { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IRootDock from)
        {
            IsFocusableRoot = from.IsFocusableRoot;
            EnableAdaptiveGlobalDockTargets = from.EnableAdaptiveGlobalDockTargets;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IRootDock to)
        {
            to.IsFocusableRoot = IsFocusableRoot;
            to.EnableAdaptiveGlobalDockTargets = EnableAdaptiveGlobalDockTargets;
        }
    }
}