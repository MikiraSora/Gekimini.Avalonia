using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.ViewModels;

public abstract class ViewModelBase : ObservableObject, IViewModel
{
    public virtual void OnViewAfterLoaded(Control view)
    {
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
    }
}