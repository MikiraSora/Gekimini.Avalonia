using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.UndoRedo;

namespace Gekimini.Avalonia.Modules.UndoRedo;

public interface IHistoryTool : IToolViewModel
{
    IUndoRedoManager UndoRedoManager { get; set; }
}