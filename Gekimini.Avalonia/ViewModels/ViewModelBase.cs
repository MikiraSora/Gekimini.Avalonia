using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, IViewModel
{
    public virtual void OnViewAfterLoaded(Control view)
    {
        IsActive = true;
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        IsActive = false;
    }
}