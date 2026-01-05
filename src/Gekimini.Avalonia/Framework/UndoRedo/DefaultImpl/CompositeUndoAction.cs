using System.Collections.Generic;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework.UndoRedo.DefaultImpl;

internal class CompositeUndoAction : IUndoableAction
{
    public CompositeUndoAction(LocalizedString name, IEnumerable<IUndoableAction> combinedActions)
    {
        CombinedActions = combinedActions;
        Name = name;
    }

    public LocalizedString Name { get; }

    public IEnumerable<IUndoableAction> CombinedActions { get; }

    public void Execute()
    {
        foreach (var subAction in CombinedActions)
            subAction.Execute();
    }

    public void Undo()
    {
        foreach (var subAction in CombinedActions)
            subAction.Undo();
    }
}