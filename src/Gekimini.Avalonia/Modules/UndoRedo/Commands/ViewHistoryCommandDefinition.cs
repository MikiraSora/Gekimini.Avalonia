using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewHistoryCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.History";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        ProgramLanguages.B.ViewHistoryCommandText.ToLocalizedString();

    public override LocalizedString ToolTip { get; } =
        ProgramLanguages.B.ViewHistoryCommandToolTip.ToLocalizedString();
}