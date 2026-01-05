using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework.UndoRedo;

public interface IUndoRedoManager : INotifyPropertyChanged
{
    ObservableCollection<IUndoableAction> ActionStack { get; }
    IUndoableAction CurrentAction { get; }
    int UndoActionCount { get; }
    int RedoActionCount { get; }

    int? UndoCountLimit { get; set; }

    bool CanUndo { get; }

    bool CanRedo { get; }

    event EventHandler BatchBegin;
    event EventHandler BatchEnd;

    void ExecuteAction(IUndoableAction action);
    void Undo(int actionCount);
    void UndoTo(IUndoableAction action);
    void UndoAll();
    void Redo(int actionCount);
    void RedoTo(IUndoableAction action);

    void Clear();

    void BeginCombineAction();
    IUndoableAction EndCombineAction(LocalizedString compositedActionName);
}