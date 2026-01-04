using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Core;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework.Tools;

public abstract partial class ToolViewModelBase : ViewModelBase, IToolViewModel,
    IRecipient<CurrentCultureInfoChangedEvent>
{
    [ObservableProperty]
    private DockMode dock;

    [ObservableProperty]
    private LocalizedString title;

    public ToolViewModelBase(LocalizedString localizedTitle)
    {
        Title = localizedTitle;
    }

    public virtual void Receive(CurrentCultureInfoChangedEvent message)
    {
        OnPropertyChanged(nameof(Title));
    }
}