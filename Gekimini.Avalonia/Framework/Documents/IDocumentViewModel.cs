using Dock.Model.Controls;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public interface IDocumentViewModel : IDocument, IViewModel
{
    IUndoRedoManager UndoRedoManager { get; }
}