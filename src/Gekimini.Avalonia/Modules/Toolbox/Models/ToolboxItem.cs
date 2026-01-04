using System;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Modules.Toolbox.Models;

public abstract class ToolboxItem
{
    public virtual string DocumentType { get; }
    public virtual LocalizedString Name { get; }
    public virtual LocalizedString Category { get; }
    public virtual string CategoryGroupId { get; }
    public virtual Uri IconSource { get; }
    public virtual string ItemType { get; }
}

public class ToolboxItem<DOCUMENT> : ToolboxItem where DOCUMENT : IDocumentViewModel
{
    public override string DocumentType => typeof(DOCUMENT).FullName;
}

public class ToolboxItem<DOCUMENT, ITEM> : ToolboxItem<DOCUMENT> where DOCUMENT : IDocumentViewModel
{
    public override string ItemType => typeof(ITEM).FullName;
}