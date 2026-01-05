using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
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
        ProgramLanguages.B.FileExitCommandText.ToLocalizedString();

    public override LocalizedString ToolTip { get; } =
        ProgramLanguages.B.FileExitCommandToolTip.ToLocalizedString();
}