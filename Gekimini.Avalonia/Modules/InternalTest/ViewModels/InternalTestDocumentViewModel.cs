using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Modules.InternalTest.ViewModels;

public partial class InternalTestDocumentViewModel : DocumentViewModelBase
{
    public InternalTestDocumentViewModel(IUndoRedoManagerFactory undoRedoManagerFactory) : base(undoRedoManagerFactory)
    {
    }

    [ObservableProperty]
    public partial int Value { get; set; }

    [RelayCommand]
    private void Increment()
    {
        var beforeValue = Value;
        UndoRedoManager.ExecuteAction(LambdaUndoAction.Create("Increment Value",
            () => Value++,
            () => Value = beforeValue));
    }

    [RelayCommand]
    private void Decrement()
    {
        var beforeValue = Value;
        UndoRedoManager.ExecuteAction(LambdaUndoAction.Create("Decrement Value",
            () => Value--,
            () => Value = beforeValue));
    }
    
    [RelayCommand]
    private void Undo()
    {
        UndoRedoManager.Undo(1);
    }
    
    [RelayCommand]
    private void Redo()
    {
        UndoRedoManager.Redo(1);
    }
}