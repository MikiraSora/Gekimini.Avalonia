using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gemini.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class SwitchToDocumentCommandListDefinition : CommandListDefinition
{
    public const string CommandName = "Window.SwitchToDocument";

    public override string Name => CommandName;
}