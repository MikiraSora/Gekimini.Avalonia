using CommunityToolkit.Mvvm.Messaging;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Models.Events;

namespace Gekimini.Avalonia.Framework.Tools;

/// <summary>
///     internal use for gekimini.*
/// todo: should we re-implement Tool?
/// </summary>
public class GekiminiToolViewModelBase : ToolViewModelBase, IRecipient<CurrentCultureInfoChangedEvent>
{
    protected readonly LocalizedString localizedTitle;

    public GekiminiToolViewModelBase(LocalizedString localizedTitle)
    {
        this.localizedTitle = localizedTitle;
        Title = this.localizedTitle?.Text ?? string.Empty;
    }

    public virtual void Receive(CurrentCultureInfoChangedEvent message)
    {
        Title = localizedTitle?.Text ?? string.Empty;
    }
}