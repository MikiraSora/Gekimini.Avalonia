using Dock.Model.Controls;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public interface IDocumentViewModel : IDocument, IViewModel
{
    IUndoRedoManager UndoRedoManager { get; }
}