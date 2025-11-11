using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gemini.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class NewFileCommandListDefinition : CommandListDefinition
{
    public const string CommandName = "File.NewFile";

    public override string Name => CommandName;
}