using System;
using Avalonia.Controls;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public abstract class DocumentViewModelBase : Document, IDocumentViewModel
{
    public DocumentViewModelBase(IUndoRedoManagerFactory undoRedoManagerFactory)
    {
        Id = Guid.NewGuid().ToString();
        Title = GetType().Name;
        UndoRedoManager = undoRedoManagerFactory.Create();
    }

    private IUndoRedoManager UndoRedoManager { get; }
    
    public virtual void OnViewAfterLoaded(Control view)
    {
        
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        
    }
}