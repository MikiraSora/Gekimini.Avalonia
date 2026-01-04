using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Gekimini.Avalonia.Modules.ToolBars.Models;
using Gekimini.Avalonia.Modules.ToolBars.ViewModels;

namespace Gekimini.Avalonia.Modules.ToolBars;

public interface IToolBar : IList<ToolBarItemViewModelBase>, IEnumerable<ToolBarItemViewModelBase>, IReadOnlyList<ToolBarItemViewModelBase>,
    INotifyCollectionChanged, INotifyPropertyChanged
{
}