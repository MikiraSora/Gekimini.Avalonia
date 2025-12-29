using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
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

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.FileExitCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.FileExitCommandToolTip));
}