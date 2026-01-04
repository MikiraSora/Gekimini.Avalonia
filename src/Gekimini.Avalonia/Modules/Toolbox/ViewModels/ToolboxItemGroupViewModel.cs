using Gekimini.Avalonia.Modules.Toolbox.Models;

namespace Gekimini.Avalonia.Modules.Toolbox.ViewModels;

public class ToolboxItemGroupViewModel
{
    public ToolboxItemGroupViewModel(ToolboxItemViewModel[] models, string name)
    {
        Models = models;
        Name = name;
    }

    public string Name { get; }

    public ToolboxItemViewModel[] Models { get; }
}