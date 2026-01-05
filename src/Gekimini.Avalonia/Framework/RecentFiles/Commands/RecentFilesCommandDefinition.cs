using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class RecentFilesCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.RecentFiles";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } = ProgramLanguages.B.CommandRecentFiles.ToLocalizedString();

    public override LocalizedString ToolTip { get; } = string.Empty.ToLocalizedStringByRawText();
}