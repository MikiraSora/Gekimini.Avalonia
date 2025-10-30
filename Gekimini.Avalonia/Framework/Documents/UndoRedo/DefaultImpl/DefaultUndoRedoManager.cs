using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.Framework.Documents.UndoRedo.DefaultImpl;

public class DefaultUndoRedoManager : ObservableObject, IUndoRedoManager
{
    private readonly Stack<List<IUndoableAction>> _combineStack = new();

    private int _undoActionCount;

    private int? _undoCountLimit;

    public ObservableCollection<IUndoableAction> ActionStack { get; } = new();

    public IUndoableAction CurrentAction => UndoActionCount > 0 ? ActionStack[UndoActionCount - 1] : null;

    public event EventHandler BatchBegin;
    public event EventHandler BatchEnd;

    public int UndoActionCount
    {
        get => _undoActionCount;

        private set
        {
            if (_undoActionCount == value)
                return;

            _undoActionCount = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(RedoActionCount));
            OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
        }
    }

    public int RedoActionCount => ActionStack.Count - UndoActionCount;

    public int? UndoCountLimit
    {
        get => _undoCountLimit;

        set
        {
            _undoCountLimit = value;
            EnforceLimit();
        }
    }

    public void BeginCombineAction()
    {
        _combineStack.Push(new List<IUndoableAction>());
    }

    public IUndoableAction EndCombineAction(string name)
    {
        if (!_combineStack.TryPop(out var combineSet))
            throw new Exception("Can't call EndCombineAction() before BeginCombineAction()");

        var compositeAction = new CompositeUndoAction(name, combineSet);
        return compositeAction;
    }

    public void ExecuteAction(IUndoableAction action)
    {
        if (_combineStack.TryPeek(out var combineSet))
        {
            combineSet.Add(action);
            return;
        }

        if (UndoActionCount < ActionStack.Count)
        {
            // We currently have items that can be redone, remove those
            for (var i = ActionStack.Count - 1; i >= UndoActionCount; i--)
                ActionStack.RemoveAt(i);

            OnPropertyChanged(nameof(RedoActionCount));
            OnPropertyChanged(nameof(CanRedo));
        }

        action.Execute();
        ActionStack.Add(action);
        UndoActionCount++;

        EnforceLimit();
    }

    public bool CanUndo => UndoActionCount > 0;

    public void Undo(int actionCount)
    {
        if (actionCount <= 0 || actionCount > UndoActionCount)
            throw new ArgumentOutOfRangeException(nameof(actionCount));

        OnBegin();

        try
        {
            for (var i = 1; i <= actionCount; i++)
                ActionStack[UndoActionCount - i].Undo();

            UndoActionCount -= actionCount;
        }
        finally
        {
            OnEnd();
        }
    }

    public void UndoTo(IUndoableAction action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (UndoActionCount < 1)
            throw new InvalidOperationException();

        // Find the action first to prevent endless loops and to only update UndoActions once
        // Do the loop in reverse from the end of the undo actions to skip searching any redo actions
        var i = UndoActionCount - 1;
        for (; i >= 0; i--)
            if (ActionStack[i] == action)
                break;

        if (i < 0)
            throw new InvalidOperationException();

        Undo(UndoActionCount - i - 1);
    }

    public void UndoAll()
    {
        if (UndoActionCount <= 0)
            return;

        for (var i = UndoActionCount - 1; i >= 0; i--)
            ActionStack[i].Undo();

        UndoActionCount = 0;
    }

    public bool CanRedo => RedoActionCount > 0;

    public void Redo(int actionCount)
    {
        if (actionCount <= 0 || actionCount > RedoActionCount)
            throw new ArgumentOutOfRangeException(nameof(actionCount));

        OnBegin();

        try
        {
            for (var i = 0; i < actionCount; i++)
                ActionStack[UndoActionCount + i].Execute();

            UndoActionCount += actionCount;
        }
        finally
        {
            OnEnd();
        }
    }

    public void RedoTo(IUndoableAction action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (RedoActionCount < 1)
            throw new InvalidOperationException();

        // Find the action first to prevent endless loops and to only update UndoActions once
        // Do the loop from the end of the undo actions to skip searching any undo actions
        var i = UndoActionCount;
        for (; i < ActionStack.Count; i++)
            if (ActionStack[i] == action)
                break;

        if (i >= ActionStack.Count)
            throw new InvalidOperationException();

        Redo(1 + i - UndoActionCount);
    }

    public void Clear()
    {
        ActionStack.Clear();
        UndoActionCount = 0;
    }

    private void EnforceLimit()
    {
        if (!UndoCountLimit.HasValue)
            return;

        var removeCount = ActionStack.Count - UndoCountLimit.Value;
        if (removeCount <= 0)
            return;

        for (var i = 0; i < removeCount; i++)
            ActionStack.RemoveAt(0);
        UndoActionCount -= removeCount;
    }

    private void OnBegin()
    {
        BatchBegin?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnd()
    {
        BatchEnd?.Invoke(this, EventArgs.Empty);
    }
}