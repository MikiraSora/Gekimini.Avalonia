using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Modules.Toolbox.Models;

namespace Gekimini.Avalonia.Modules.Toolbox.ViewModels;

public class ToolboxItemGroupViewModel
{
    public ToolboxItemGroupViewModel(ToolboxItemViewModel[] models, LocalizedString name)
    {
        Models = models;
        Name = name;
    }

    public LocalizedString Name { get; }

    public ToolboxItemViewModel[] Models { get; }
}