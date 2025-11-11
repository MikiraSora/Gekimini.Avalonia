using System;
using Avalonia.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework;

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
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
}