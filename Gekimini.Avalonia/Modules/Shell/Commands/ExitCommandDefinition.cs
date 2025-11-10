using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ExitCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.Exit";

    [RegisterStaticObject<CommandKeyboardShortcut>]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture(Key.F4, KeyModifiers.Alt));

    public override string Name => CommandName;

    public override string Text => "Resources.FileExitCommandText";

    public override string ToolTip => "Resources.FileExitCommandToolTip";
}