using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class CloseFileCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.CloseFile";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        ProgramLanguages.B.FileCloseCommandText.ToLocalizedString();

    public override LocalizedString ToolTip { get; } =
        ProgramLanguages.B.FileCloseCommandToolTip.ToLocalizedString();
}