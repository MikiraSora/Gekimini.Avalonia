using System;
using Gekimini.Avalonia.Modules.Toolbox.Models;

namespace Gekimini.Avalonia.Modules.Toolbox.ViewModels;

public class ToolboxItemViewModel
{
    public ToolboxItemViewModel(ToolboxItem model)
    {
        Model = model;
    }

    public ToolboxItem Model { get; }

    public string Name => Model.Name;

    public virtual string Category => Model.Category;

    public virtual Uri IconSource => Model.IconSource;
}