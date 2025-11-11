using System;
using Avalonia.Controls;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;

namespace Gekimini.Avalonia.Framework;

public abstract class DocumentViewModelBase : Document, IDocumentViewModel
{
    public DocumentViewModelBase(IUndoRedoManagerFactory undoRedoManagerFactory)
    {
        Id = Guid.NewGuid().ToString();
        Title = GetType().Name;
        UndoRedoManager = undoRedoManagerFactory.Create();
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

    public IUndoRedoManager UndoRedoManager { get; }
}