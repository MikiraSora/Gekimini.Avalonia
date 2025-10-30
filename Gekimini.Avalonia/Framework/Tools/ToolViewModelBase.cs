using System;
using Avalonia.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public abstract class ToolViewModelBase : Tool, IToolViewModel
{
    public ToolViewModelBase()
    {
        Id = Guid.NewGuid().ToString();
        Title = GetType().Name;
        Dock = DockMode.Left;
    }
    
    public virtual void OnViewAfterLoaded(Control view)
    {
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
    }
}