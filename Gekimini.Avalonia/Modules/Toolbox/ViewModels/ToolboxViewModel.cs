using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Core;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Modules.Toolbox.Services;
using Gekimini.Avalonia.Utils;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Toolbox.ViewModels;

[RegisterSingleton<IToolbox>]
public partial class ToolboxViewModel : ToolViewModelBase, IToolbox
{
    private readonly ObservableCollection<ToolboxItemGroupViewModel> _filteredItems = new();
    private readonly ObservableCollection<ToolboxItemGroupViewModel> _groupedItems = new();

    private readonly ObservableCollection<ToolboxItemViewModel> _items = new();

    private readonly IToolboxService _toolboxService;

    public ToolboxViewModel(IShell shell, IToolboxService toolboxService)
    {
        Dock = DockMode.Left;
        Title = Resources.ToolboxDisplayName;

        _toolboxService = toolboxService;

        if (DesignModeHelper.IsDesignMode)
            return;

        shell.ActiveDocumentChanged += (sender, e) => RefreshToolboxItems(shell);
        RefreshToolboxItems(shell);
    }

    public ObservableCollection<ToolboxItemGroupViewModel> Items =>
        _filteredItems.Count == 0 ? _groupedItems : _filteredItems;

    private void RefreshToolboxItems(IShell shell)
    {
        _items.Clear();
        _groupedItems.Clear();
        _filteredItems.Clear();

        if (shell.ActiveDocument is not null)
        {
            _items.AddRange(_toolboxService.GetToolboxItems(shell.ActiveDocument.GetType())
                .Select(x => new ToolboxItemViewModel(x)));
            _groupedItems.AddRange(_items.GroupBy(x => x.Category)
                .Select(x => new ToolboxItemGroupViewModel(x.ToArray(), x.Key)));
        }

        OnPropertyChanged(nameof(Items));
    }

    [RelayCommand]
    private void Search(string searchTerm)
    {
        var filters = new List<ToolboxItemViewModel>();
        if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 2)
            foreach (var item in _items.Where(x =>
                         x.Name.ToUpper().Contains(searchTerm.ToUpper()) ||
                         x.Name.ToUpper().Equals(searchTerm.ToUpper())))
                filters.Add(item);

        _filteredItems.Clear();
        _filteredItems.AddRange(_items.GroupBy(x => x.Category)
            .Select(x => new ToolboxItemGroupViewModel(x.ToArray(), x.Key + " (Filtered)")));

        OnPropertyChanged(nameof(Items));
    }
}