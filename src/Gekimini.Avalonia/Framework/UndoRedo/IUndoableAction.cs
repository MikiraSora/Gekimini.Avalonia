
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework.UndoRedo
{
    public interface IUndoableAction
    {
        LocalizedString Name { get; }

        void Execute();
        void Undo();
    }
}
