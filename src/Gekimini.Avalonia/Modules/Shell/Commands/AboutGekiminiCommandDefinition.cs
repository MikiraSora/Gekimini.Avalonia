using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class AboutGekiminiCommandDefinition : CommandDefinition
{
    public const string CommandName = "Help.AboutGekimini";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromRawText("About Gekimini");

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromRawText("Check information about framework Gekimini");
}