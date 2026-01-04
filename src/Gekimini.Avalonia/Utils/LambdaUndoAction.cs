using System;
using Gekimini.Avalonia.Framework.UndoRedo;

namespace Gekimini.Avalonia.Utils;

public class LambdaUndoAction : IUndoableAction
{
    private readonly Action executeOrRedo;
    private readonly Action undo;

    public LambdaUndoAction(string actionName, Action executeOrRedo, Action undo)
    {
        this.Name = actionName;
        this.executeOrRedo = executeOrRedo;
        this.undo = undo;
    }

    public string Name { get; }

    public void Execute()
    {
        executeOrRedo();
    }

    public void Undo()
    {
        undo();
    }

    public static LambdaUndoAction Create(string actionName, Action executeOrRedo, Action undo)
    {
        return new LambdaUndoAction(actionName, executeOrRedo, undo);
    }
}