using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Modules.Documents.Models;
using Gekimini.Avalonia.Modules.Documents.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Models;

[RegisterSingleton<IFactory>]
public sealed partial class ShellDockFactory : Factory, IFactory
{
    private IRootDock root;

    public ShellDockFactory()
    {
        HideToolsOnClose = true;
        HideDocumentsOnClose = true;
    }

    [GetServiceLazy]
    private partial IDialogManager DialogManager { get; }

    public override IRootDock CreateLayout()
    {
        var root = CreateRootDock();

        var leftToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Left,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            Dock = DockMode.Left,
            CanFloat = false
        };
        var documentDock = new DocumentDock
        {
            VisibleDockables = CreateList<IDockable>(),
            IsCollapsable = false,
            Proportion = 0.5,
            CanFloat = false,
            Dock = DockMode.Center
        };
        var rightToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Right,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            CanFloat = false,
            Dock = DockMode.Right
        };
        var bottomToolDock = new ToolDock
        {
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Bottom,
            CanClose = false,
            Proportion = 0.25,
            IsCollapsable = true,
            CanFloat = false,
            Dock = DockMode.Bottom
        };
        var topToolDock = new ToolDock
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

    public override IDockWindow CreateWindowFrom(IDockable dockable)
    {
        var window = base.CreateWindowFrom(dockable);

        if (window != null)
            window.Title = "Dock Avalonia Demo";
        return window;
    }

    public override void InitLayout(IDockable layout)
    {
        base.InitLayout(layout);
        root = FindRoot(layout);
    }

    public override async void CloseDockable(IDockable dockable)
    {
        if (dockable is IDocument document)
            if (!await CanCloseDocument(document))
                //todo log it
                return;

        base.CloseDockable(dockable);
    }

    private IDock FindOrCreateToolDock(DockMode dock)
    {
        var toolDock =
            Find(x => x is IDock)
                .OfType<IDock>()
                .Select(x => new
                {
                    Rank = CalculateRank(x),
                    Dock = x
                })
                .OrderByDescending(x => x.Rank)
                .FirstOrDefault()?
                .Dock;

        if (toolDock is null)
        {
            //create default
            toolDock = CreateToolDock();
            toolDock.Dock = dock;

            //todo log
            AddDockable(root, toolDock);
        }

        return toolDock;

        int CalculateRank(IDock dd)
        {
            return dd switch
            {
                IToolDock when dd.Dock == dock => 3,
                IToolDock => 2,
                _ => dd.Dock == dock ? 1 : 0
            };
        }
    }

    private IDock FindOrCreateDocumentDock(DockMode dock)
    {
        var documentDock =
            Find(x => x is IDock)
                .OfType<IDock>()
                .Select(x => new
                {
                    Rank = CalculateRank(x),
                    Dock = x
                })
                .OrderByDescending(x => x.Rank)
                .FirstOrDefault()?
                .Dock;

        if (documentDock is null)
        {
            //create default
            documentDock = CreateDocumentDock();
            documentDock.Dock = dock;

            //todo log
            AddDockable(root, documentDock);
        }

        return documentDock;

        int CalculateRank(IDock dd)
        {
            if (dd is IDocumentDock && dd.Dock == dock)
                return 3;

            if (dd is IDocumentDock)
                return 2;

            if (dd.Dock == dock)
                return 1;

            return 0;
        }
    }

    public void AddDocument(IDocument dockable)
    {
        var documentDock = FindOrCreateDocumentDock(dockable.Dock);

        AddDockable(documentDock, dockable);

        ActiveAndFocus(dockable);
    }

    public void ActiveAndFocus(IDockable dockable)
    {
        SetActiveDockable(dockable);
        if (dockable.Owner is IDock dock)
            SetFocusedDockable(dock, dockable);
    }

    public void AddTool(ITool dockable)
    {
        dockable.CanFloat = false;

        var toolDock = FindOrCreateToolDock(dockable.Dock);

        AddDockable(toolDock, dockable);

        ActiveAndFocus(dockable);
        //toolDock.ActiveDockable = dockable;
        //toolDock.FocusedDockable = dockable;
    }

    public void RemoveTool(ITool dockable)
    {
        RemoveDockable(dockable, false);
    }

    public void RemoveDocument(IDocument dockable)
    {
        RemoveDockable(dockable, false);
    }

    public async Task<bool> CanCloseDocument(IDocument document)
    {
        if (document is not IPersistedDocumentViewModel persistedDocumentViewModel)
            return true;

        if (!persistedDocumentViewModel.IsDirty)
            return true;

        var dialog = new SaveDirtyDocumentDialogViewModel
        {
            DocumentName = document.Title
        };
        await DialogManager.ShowDialog(dialog);

        switch (dialog.Result)
        {
            case DialogResult.Yes:
                var isSaveSuccess = await persistedDocumentViewModel.Save();
                if (!isSaveSuccess)
                {
                    await DialogManager.ShowMessageDialog(
                        $"Document {document.Title} saving is failed/canceled, application exit has been canceled.");
                    return false;
                }

                break;
            case DialogResult.No:
                //user cancel save this document, just continue.
                break;
            case DialogResult.Cancel:
            default:
                //user cancel application exit process.
                return false;
        }

        return true;
    }
}