using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
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

    public override string Text => Resources.ViewFullScreenCommandText;

    public override string ToolTip => Resources.ViewFullScreenCommandToolTip;

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/FullScreen.png");
}