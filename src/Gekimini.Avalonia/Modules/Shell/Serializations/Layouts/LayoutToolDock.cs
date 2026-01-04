using System.Collections.Generic;
using System.Windows.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutToolDock : LayoutDock
{
    public Alignment Alignment { get; set; }
    public bool IsExpanded { get; set; }
    public bool AutoHide { get; set; }
    public GripMode GripMode { get; set; }
    
    public override void CopyFrom(IDockable f)
    {
        base.CopyFrom(f);
        if (f is IToolDock from)
        {
            Alignment = from.Alignment;
            IsExpanded = from.IsExpanded;
            GripMode = from.GripMode;
            AutoHide = from.AutoHide;
        }
    }

    public override void CopyTo(IDockable t)
    {
        base.CopyTo(t);
        if (t is IToolDock to)
        {
            to.Alignment = Alignment;
            to.IsExpanded = IsExpanded;
            to.AutoHide = AutoHide;
            to.GripMode = GripMode;
        }
    }
}