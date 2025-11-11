using Gekimini.Avalonia.Framework.Documents.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.UndoRedo.ViewModels;

public class HistoryItemViewModel : ViewModelBase
{
    private readonly string _name;

    private HistoryItemType _itemType;

    public HistoryItemViewModel(IUndoableAction action)
    {
        Action = action;
    }

    public HistoryItemViewModel(string name)
    {
        _name = name;
    }

    public IUndoableAction Action { get; }

    public string Name => _name ?? Action.Name;

    public HistoryItemType ItemType
    {
        get => _itemType;
        set
        {
            if (_itemType == value)
                return;

            _itemType = value;
            OnPropertyChanged();
        }
    }
}