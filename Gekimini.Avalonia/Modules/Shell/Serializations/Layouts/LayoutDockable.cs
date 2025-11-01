using System.Text.Json.Serialization;
using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "LayoutType")]
[JsonDerivedType(typeof(LayoutDockDock), nameof(LayoutDockDock))]
[JsonDerivedType(typeof(LayoutRootDock), nameof(LayoutRootDock))]
[JsonDerivedType(typeof(LayoutStackDock), nameof(LayoutStackDock))]
[JsonDerivedType(typeof(LayoutGridDock), nameof(LayoutGridDock))]
[JsonDerivedType(typeof(LayoutProportionalDock), nameof(LayoutProportionalDock))]
[JsonDerivedType(typeof(LayoutToolDock), nameof(LayoutToolDock))]
[JsonDerivedType(typeof(LayoutDocumentDock), nameof(LayoutDocumentDock))]
[JsonDerivedType(typeof(LayoutUniformGridDock), nameof(LayoutUniformGridDock))]
[JsonDerivedType(typeof(LayoutWrapDock), nameof(LayoutWrapDock))]
[JsonDerivedType(typeof(LayoutDock), nameof(LayoutDock))]
[JsonDerivedType(typeof(LayoutDocument), nameof(LayoutDocument))]
[JsonDerivedType(typeof(LayoutTool), nameof(LayoutTool))]
[JsonDerivedType(typeof(LayoutSplitter), nameof(LayoutSplitter))]
[JsonDerivedType(typeof(LayoutProportionalDockSplitter), nameof(LayoutProportionalDockSplitter))]
[JsonDerivedType(typeof(LayoutGridDockSplitter), nameof(LayoutGridDockSplitter))]
public class LayoutDockable
{
    public string Id { get; set; }

    public string Title { get; set; }

    //public object Context { get; set; }
    //public IDockable Owner { get; set; }
    //public IDockable OriginalOwner { get; set; }
    //public IFactory Factory { get; set; }
    //public bool IsEmpty { get; set; }
    public bool IsCollapsable { get; set; }
    public double Proportion { get; set; }
    public DockMode Dock { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }
    public bool IsSharedSizeScope { get; set; }
    public double CollapsedProportion { get; set; }
    public double MinWidth { get; set; }
    public double MaxWidth { get; set; }
    public double MinHeight { get; set; }
    public double MaxHeight { get; set; }
    public bool CanClose { get; set; }
    public bool CanPin { get; set; }
    public bool CanFloat { get; set; }
    public bool CanDrag { get; set; }
    public bool CanDrop { get; set; }
    public bool IsModified { get; set; }
    public string DockGroup { get; set; }

    public virtual void CopyTo(IDockable to)
    {
        to.Id = Id;
        to.Title = Title;
        to.IsCollapsable = IsCollapsable;
        to.Proportion = Proportion;
        to.Column = Column;
        to.Row = Row;
        to.ColumnSpan = ColumnSpan;
        to.RowSpan = RowSpan;
        to.IsSharedSizeScope = IsSharedSizeScope;
        to.CollapsedProportion = CollapsedProportion;
        to.MinWidth = MinWidth;
        to.MaxWidth = MaxWidth;
        to.MinHeight = MinHeight;
        to.MaxHeight = MaxHeight;
        to.CanClose = CanClose;
        to.CanPin = CanPin;
        to.CanDrag = CanDrag;
        to.CanDrop = CanDrop;
        to.CanFloat = CanFloat;
        to.IsModified = IsModified;
        to.DockGroup = DockGroup;
        to.Dock = Dock;
    }

    public virtual void CopyFrom(IDockable from)
    {
        Id = from.Id;
        Title = from.Title;
        IsCollapsable = from.IsCollapsable;
        Proportion = from.Proportion;
        Column = from.Column;
        Row = from.Row;
        ColumnSpan = from.ColumnSpan;
        RowSpan = from.RowSpan;
        IsSharedSizeScope = from.IsSharedSizeScope;
        CollapsedProportion = from.CollapsedProportion;
        MinWidth = from.MinWidth;
        MaxWidth = from.MaxWidth;
        MinHeight = from.MinHeight;
        MaxHeight = from.MaxHeight;
        CanClose = from.CanClose;
        CanPin = from.CanPin;
        CanDrag = from.CanDrag;
        CanDrop = from.CanDrop;
        CanFloat = from.CanFloat;
        IsModified = from.IsModified;
        DockGroup = from.DockGroup;
        Dock = from.Dock;
    }
}