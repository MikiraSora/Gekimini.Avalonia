using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class SaveFileAsCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.SaveFileAs";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } = LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.FileSaveAsCommandText));

    public override LocalizedString ToolTip { get; } = LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.FileSaveAsCommandToolTip));
}