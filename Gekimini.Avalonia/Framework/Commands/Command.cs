using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.Framework.Commands;

public partial class Command : ObservableObject
{
    public Command(CommandDefinitionBase commandDefinition)
    {
        CommandDefinition = commandDefinition;
        Text = commandDefinition.Text;
        ToolTip = commandDefinition.ToolTip;
        IconSource = commandDefinition.IconSource;
    }

    public CommandDefinitionBase CommandDefinition { get; }

    [ObservableProperty]
    public partial bool Visible { get; set; } = true;

    [ObservableProperty]
    public partial bool Enabled { get; set; } = true;

    [ObservableProperty]
    public partial bool Checked { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [ObservableProperty]
    public partial string ToolTip { get; set; }

    [ObservableProperty]
    public partial Uri IconSource { get; set; }

    public object Tag { get; set; }
}