using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Core;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.ViewModels;

[RegisterSingleton<IHistoryTool>]
public partial class HistoryViewModel : ToolViewModelBase, IHistoryTool
{
    private int _selectedIndex;
    private IUndoRedoManager _undoRedoManager;

    public HistoryViewModel(IShell shell)
    {
        Dock = DockMode.Right;
        Title = Resources.HistoryDisplayName;

        if (shell == null)
            return;

        shell.ActiveDocumentChanged += (sender, e) =>
        {
            UndoRedoManager = shell.ActiveDocument != null ? shell.ActiveDocument.UndoRedoManager : null;
        };
        if (shell.ActiveDocument != null)
            UndoRedoManager = shell.ActiveDocument.UndoRedoManager;
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex == value)
                return;

            _selectedIndex = value;
            OnPropertyChanged();
            if (HistoryItems.ElementAtOrDefault(value - 1) is { } historyItem)
                UndoOrRedoTo(historyItem, false);
        }
    }

    public ObservableCollection<HistoryItemViewModel> HistoryItems { get; } = new();

    public IUndoRedoManager UndoRedoManager
    {
        get => _undoRedoManager;
        set
        {
            if (_undoRedoManager == value)
                return;

            if (_undoRedoManager != null)
            {
                _undoRedoManager.ActionStack.CollectionChanged -= OnUndoRedoManagerActionStackChanged;
                _undoRedoManager.PropertyChanged -= OnUndoRedoManagerPropertyChanged;
            }

            _undoRedoManager = value;

            if (_undoRedoManager != null)
            {
                _undoRedoManager.ActionStack.CollectionChanged += OnUndoRedoManagerActionStackChanged;
                _undoRedoManager.PropertyChanged += OnUndoRedoManagerPropertyChanged;

                ResetItems();
            }
        }
    }

    [RelayCommand]
    private void UndoOrRedoTo(HistoryItemViewModel item)
    {
        UndoOrRedoTo(item, true);
    }

    private void ResetItems()
    {
        HistoryItems.Clear();
        HistoryItems.Add(new HistoryItemViewModel(Resources.HistoryInitialState));
        foreach (var vm in _undoRedoManager.ActionStack.Select(a => new HistoryItemViewModel(a)))
            HistoryItems.Add(vm);
        RefreshItemTypes();
    }

    private void OnUndoRedoManagerActionStackChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var newItems = e.NewItems.Cast<IUndoableAction>().ToArray();
                for (var i = 0; i < newItems.Length; i++)
                    HistoryItems.Insert(e.NewStartingIndex + i + 1, new HistoryItemViewModel(newItems[i]));
                break;
            case NotifyCollectionChangedAction.Remove:
                for (var i = 0; i < e.OldItems.Count; i++)
                    HistoryItems.RemoveAt(e.OldStartingIndex + 1);
                break;
            case NotifyCollectionChangedAction.Reset:
                ResetItems();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void OnUndoRedoManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IUndoRedoManager.UndoActionCount):
                RefreshItemTypes();
                break;
        }
    }

    private void RefreshItemTypes()
    {
        HistoryItems[0].ItemType = _undoRedoManager.CanUndo ? HistoryItemType.InitialState : HistoryItemType.Current;
        _selectedIndex = 0;
        
        var idx = 0;
        for (var i = 1; i <= _undoRedoManager.ActionStack.Count; i++)
        {
            var delta = _undoRedoManager.UndoActionCount - i;
            if (delta == 0)
            {
                HistoryItems[i].ItemType = HistoryItemType.Current;
                _selectedIndex = i;
            }
            else
            {
                HistoryItems[i].ItemType = delta > 0 ? HistoryItemType.Undo : HistoryItemType.Redo;
            }
        }

        OnPropertyChanged(nameof(SelectedIndex));
    }

    public void UndoOrRedoTo(HistoryItemViewModel item, bool setSelectedIndex)
    {
        switch (item.ItemType)
        {
            case HistoryItemType.InitialState:
                _undoRedoManager.UndoAll();
                break;
            case HistoryItemType.Undo:
                _undoRedoManager.UndoTo(item.Action);
                break;
            case HistoryItemType.Current:
                break;
            case HistoryItemType.Redo:
                _undoRedoManager.RedoTo(item.Action);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (setSelectedIndex)
            SelectedIndex = HistoryItems.IndexOf(item) + 1;
    }
}