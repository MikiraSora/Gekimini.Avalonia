using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Core;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Tools;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.ViewModels;

[RegisterSingleton<IHistoryTool>]
public partial class HistoryToolViewModel : GekiminiToolViewModelBase, IHistoryTool
{
    private int _selectedIndex;

    public HistoryToolViewModel(IShell shell) : base(
        LocalizedString.CreateFromResourceKey(nameof(Resources.HistoryDisplayName)))
    {
        Dock = DockMode.Right;

        if (shell == null)
            return;

        shell.ActiveDocumentChanged += (sender, e) => { UndoRedoManager = shell.ActiveDocument?.UndoRedoManager; };

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
        get => field;
        set
        {
            if (field == value)
                return;

            if (field != null)
            {
                field.ActionStack.CollectionChanged -= OnUndoRedoManagerActionStackChanged;
                field.PropertyChanged -= OnUndoRedoManagerPropertyChanged;
            }

            field = value;

            if (field != null)
            {
                field.ActionStack.CollectionChanged += OnUndoRedoManagerActionStackChanged;
                field.PropertyChanged += OnUndoRedoManagerPropertyChanged;
            }

            ResetItems();
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

        if (UndoRedoManager is not null)
            foreach (var vm in UndoRedoManager.ActionStack.Select(a => new HistoryItemViewModel(a)))
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
        _selectedIndex = 0;
        if (UndoRedoManager is not null)
        {
            HistoryItems[0].ItemType = UndoRedoManager.CanUndo ? HistoryItemType.InitialState : HistoryItemType.Current;

            for (var i = 1; i <= UndoRedoManager.ActionStack.Count; i++)
            {
                var delta = UndoRedoManager.UndoActionCount - i;
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
        }

        OnPropertyChanged(nameof(SelectedIndex));
    }

    private void UndoOrRedoTo(HistoryItemViewModel item, bool setSelectedIndex)
    {
        switch (item.ItemType)
        {
            case HistoryItemType.InitialState:
                UndoRedoManager.UndoAll();
                break;
            case HistoryItemType.Undo:
                UndoRedoManager.UndoTo(item.Action);
                break;
            case HistoryItemType.Current:
                break;
            case HistoryItemType.Redo:
                UndoRedoManager.RedoTo(item.Action);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (setSelectedIndex)
            SelectedIndex = HistoryItems.IndexOf(item) + 1;
    }
}