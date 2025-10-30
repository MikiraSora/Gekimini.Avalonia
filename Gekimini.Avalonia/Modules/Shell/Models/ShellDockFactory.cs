using System;
using System.Collections.Generic;
using System.Linq;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Models;

[RegisterSingleton<IFactory>]
public class ShellDockFactory : Factory, IFactory
{
    private readonly IServiceProvider serviceProvider;
    private ToolDock bottomToolDock;

    private DocumentDock documentDock;

    private ToolDock leftToolDock;
    private ToolDock rightToolDock;
    private ToolDock topToolDock;

    public ShellDockFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public override IRootDock CreateLayout()
    {
        var root = CreateRootDock();

        leftToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Left,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            Dock = DockMode.Left,
            CanFloat = false
        };
        documentDock = new DocumentDock
        {
            VisibleDockables = CreateList<IDockable>(),
            IsCollapsable = false,
            Proportion = 0.5,
            CanFloat = false,
            Dock = DockMode.Center
        };
        rightToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Right,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            CanFloat = false,
            Dock = DockMode.Right
        };
        bottomToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Bottom,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            CanFloat = false,
            Dock = DockMode.Bottom
        };
        topToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Top,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            CanFloat = false,
            Dock = DockMode.Top
        };

        var horizonLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftToolDock,
                new ProportionalDockSplitter(),
                documentDock,
                new ProportionalDockSplitter(),
                rightToolDock
            ),
            CanFloat = false,
            Proportion = 0.5
        };

        var verticalLayout = new ProportionalDock
        {
            Orientation = Orientation.Vertical,
            VisibleDockables = CreateList<IDockable>
            (
                topToolDock,
                new ProportionalDockSplitter(),
                horizonLayout,
                new ProportionalDockSplitter(),
                bottomToolDock
            ),
            CanFloat = false
        };

        var mainLayout = verticalLayout;

        root.VisibleDockables = CreateList<IDockable>(mainLayout);
        root.IsCollapsable = false;
        root.ActiveDockable = mainLayout;
        root.DefaultDockable = mainLayout;
        root.PinnedDock = null;
        root.CanFloat = false;

        return root;
    }

    public override IList<T> CreateList<T>(params T[] items)
    {
        return base.CreateList(items.Where(x => x != null).ToArray());
    }

    public override IDockWindow? CreateWindowFrom(IDockable dockable)
    {
        var window = base.CreateWindowFrom(dockable);

        if (window != null)
            window.Title = "Dock Avalonia Demo";
        return window;
    }

    public override void InitLayout(IDockable layout)
    {
        base.InitLayout(layout);
    }

    public void AddDocument(IDocument dockable)
    {
        dockable.CanFloat = false;
        
        AddDockable(documentDock,dockable);
        //documentDock.AddDocument(dockable);

        documentDock.ActiveDockable = dockable;
        documentDock.FocusedDockable = dockable;
    }
    
    public void RemoveDocument(IDocument dockable)
    {
        RemoveDockable(dockable, true);
    }

    public void AddTool(ITool dockable)
    {
        dockable.CanFloat = false;

        IDock addDock = dockable.Dock switch
        {
            DockMode.Center => documentDock,
            DockMode.Left => leftToolDock,
            DockMode.Bottom => bottomToolDock,
            DockMode.Right => rightToolDock,
            DockMode.Top => topToolDock,
            _ => throw new ArgumentOutOfRangeException()
        };


        AddDockable(addDock,dockable);
        //AddVisibleDockable(addDock, dockable);
        
        addDock.ActiveDockable = dockable;
        addDock.FocusedDockable = dockable;
    }

    public void RemoveTool(ITool dockable)
    {
        RemoveDockable(dockable, true);
    }
}