using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class SaveFileAsCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.SaveFileAs";

    public override string Name => CommandName;

    public override string Text => Resources.FileSaveAsCommandText;

    public override string ToolTip => Resources.FileSaveAsCommandToolTip;
}