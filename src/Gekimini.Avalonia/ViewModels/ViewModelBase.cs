using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.ViewModels;

public abstract partial class ViewModelBase : ObservableRecipient, IViewModel
{
    public virtual void OnViewAfterLoaded(IView view)
    {
        IsActive = true;
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(IView view)
    {
        ViewBeforeUnload?.Invoke(view);
        IsActive = false;
    }

    public event Action<IView> ViewAfterLoaded;
    public event Action<IView> ViewBeforeUnload;
}