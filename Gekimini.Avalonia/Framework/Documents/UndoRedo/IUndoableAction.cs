
namespace Gekimini.Avalonia.Framework.Documents.UndoRedo
{
    public interface IUndoableAction
    {
        string Name { get; }

        void Execute();
        void Undo();
    }
}
