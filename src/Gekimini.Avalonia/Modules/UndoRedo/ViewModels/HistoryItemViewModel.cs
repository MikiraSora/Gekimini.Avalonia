using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.UndoRedo.ViewModels;

public class HistoryItemViewModel : ViewModelBase
{
    private readonly LocalizedString _name;

    private HistoryItemType _itemType;

    public HistoryItemViewModel(IUndoableAction action)
    {
        Action = action;
    }

    public HistoryItemViewModel(LocalizedString name)
    {
        _name = name;
    }

    public IUndoableAction Action { get; }

    public LocalizedString Name => _name ?? Action.Name;

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