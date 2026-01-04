using System;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Modules.Toolbox.Models;

namespace Gekimini.Avalonia.Modules.Toolbox.ViewModels;

public class ToolboxItemViewModel
{
    public ToolboxItemViewModel(ToolboxItem model)
    {
        Model = model;
    }

    public ToolboxItem Model { get; }

    public LocalizedString Name => Model.Name;

    public virtual LocalizedString Category => Model.Category;
    
    public virtual string CategoryGroupId => Model.CategoryGroupId;

    public virtual Uri IconSource => Model.IconSource;
}