using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class RecentFilesCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.RecentFiles";

    public override string Name => CommandName;

    public override string Text => Resources.CommandRecentFiles;

    public override string ToolTip => string.Empty;
}