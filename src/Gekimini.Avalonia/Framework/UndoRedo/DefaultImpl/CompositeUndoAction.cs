using System.Collections.Generic;

namespace Gekimini.Avalonia.Framework.UndoRedo.DefaultImpl;

internal class CompositeUndoAction : IUndoableAction
{
    public CompositeUndoAction(string name, IEnumerable<IUndoableAction> combinedActions)
    {
        CombinedActions = combinedActions;
        Name = name;
    }

    public string Name { get; }

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