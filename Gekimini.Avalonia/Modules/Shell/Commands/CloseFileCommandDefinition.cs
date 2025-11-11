using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class CloseFileCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.CloseFile";

    public override string Name => CommandName;

    public override string Text => Resources.FileCloseCommandText;

    public override string ToolTip => Resources.FileCloseCommandToolTip;
}