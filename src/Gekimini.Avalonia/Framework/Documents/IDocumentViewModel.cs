using Gekimini.Avalonia.Framework.Documents;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public interface IDocumentViewModel : IDockableViewModel
{
    IUndoRedoManager UndoRedoManager { get; }
}