using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ResetLayoutCommandDefinition : CommandDefinition
{
    public const string CommandName = "Layout.ResetLayout";

    public override string Name => CommandName;

    public override string Text => "Reset Layout";

    public override string ToolTip => "Clean all layout and your tools will be removed.";
}