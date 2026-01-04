using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ResetLayoutCommandDefinition : CommandDefinition
{
    public const string CommandName = "Layout.ResetLayout";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } = "Reset Layout".ToLocalizedStringByRawText();

    public override LocalizedString ToolTip { get; } =
        "Clean all layout and your tools will be removed.".ToLocalizedStringByRawText();
}