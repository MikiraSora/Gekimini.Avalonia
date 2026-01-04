using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewHistoryCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.History";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.ViewHistoryCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.ViewHistoryCommandToolTip));
}