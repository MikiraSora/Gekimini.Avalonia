using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Toolbox.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewToolboxCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.Toolbox";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        ProgramLanguages.B.ViewToolboxCommandText.ToLocalizedString();

    public override LocalizedString ToolTip { get; } =
        ProgramLanguages.B.ViewToolboxCommandToolTip.ToLocalizedString();
}