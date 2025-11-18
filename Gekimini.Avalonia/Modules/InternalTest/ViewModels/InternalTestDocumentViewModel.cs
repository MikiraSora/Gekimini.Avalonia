using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.DragDrops;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Utils;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.InternalTest.ViewModels;

public partial class InternalTestDocumentViewModel : DocumentViewModelBase
{
    private readonly IDragDropManager _dragDropManager;
    private readonly ILogger<InternalTestDocumentViewModel> _logger;

    public InternalTestDocumentViewModel(IUndoRedoManagerFactory undoRedoManagerFactory,
        IDragDropManager dragDropManager, ILogger<InternalTestDocumentViewModel> logger) : base(undoRedoManagerFactory)
    {
        _dragDropManager = dragDropManager;
        _logger = logger;
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

    public override void OnViewAfterLoaded(Control view)
    {
        base.OnViewAfterLoaded(view);

        DragDrop.SetAllowDrop(view, true);
        DragDrop.AddDropHandler(view, OnDragDrop);
    }

    public override void OnViewBeforeUnload(Control view)
    {
        base.OnViewBeforeUnload(view);

        DragDrop.RemoveDropHandler(view, OnDragDrop);
        DragDrop.SetAllowDrop(view, false);
    }

    private void OnDragDrop(object sender, DragEventArgs e)
    {
        if (_dragDropManager.TryGetDragData(e, out var data))
        {
            _logger.LogDebugEx($"toolboxitem: {data}");
            _dragDropManager.EndDragDropEvent(e);
        }
    }
}