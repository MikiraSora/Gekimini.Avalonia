using System;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.UndoRedo;

namespace Gekimini.Avalonia.Utils;

public class LambdaUndoAction : IUndoableAction
{
    private readonly Action executeOrRedo;
    private readonly Action undo;

    public LambdaUndoAction(LocalizedString actionName, Action executeOrRedo, Action undo)
    {
        this.Name = actionName;
        this.executeOrRedo = executeOrRedo;
        this.undo = undo;
    }

    public LocalizedString Name { get; }

    public void Execute()
    {
        executeOrRedo();
    }

    public void Undo()
    {
        undo();
    }

    public static LambdaUndoAction Create(LocalizedString actionName, Action executeOrRedo, Action undo)
    {
        return new LambdaUndoAction(actionName, executeOrRedo, undo);
    }
}