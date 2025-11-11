using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, IViewModel
{
    public virtual void OnViewAfterLoaded(Control view)
    {
        IsActive = true;
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
        IsActive = false;
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
}