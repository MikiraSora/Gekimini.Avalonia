using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.ViewModels;

public abstract class ViewModelBase : ObservableObject, IViewModel
{
    public virtual void OnViewAfterLoaded(Control view)
    {
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
}