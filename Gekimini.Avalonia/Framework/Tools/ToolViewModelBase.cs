using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;

namespace Gekimini.Avalonia.Framework.Tools;

public abstract class ToolViewModelBase : Tool, IToolViewModel
{
    public ToolViewModelBase()
    {
        Id = GetType().FullName;
        Title = GetType().Name;
        Dock = DockMode.Left;
    }

    public virtual void OnViewAfterLoaded(Control view)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
}