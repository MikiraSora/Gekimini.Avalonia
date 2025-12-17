using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Framework.Tools;

public abstract class ToolViewModelBase : Tool, IToolViewModel
{
    public ToolViewModelBase()
    {
        Id = GetType().FullName;
        Title = GetType().Name;
        Dock = DockMode.Left;
    }

    public virtual void OnViewAfterLoaded(IView view)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(IView view)
    {
        ViewBeforeUnload?.Invoke(view);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public event Action<IView> ViewAfterLoaded;
    public event Action<IView> ViewBeforeUnload;
}