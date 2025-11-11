using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewHistoryCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.History";

    public override string Name => CommandName;

    public override string Text => Resources.ViewHistoryCommandText;

    public override string ToolTip => Resources.ViewHistoryCommandToolTip;
}