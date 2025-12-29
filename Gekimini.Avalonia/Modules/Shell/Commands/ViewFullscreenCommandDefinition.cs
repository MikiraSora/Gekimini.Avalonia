using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewFullScreenCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.FullScreen";

    [RegisterStaticObject]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<ViewFullScreenCommandDefinition>(new KeyGesture(Key.F5,
            KeyModifiers.Shift | KeyModifiers.Alt));

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.ViewFullScreenCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.ViewFullScreenCommandToolTip));

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/FullScreen.png");
}