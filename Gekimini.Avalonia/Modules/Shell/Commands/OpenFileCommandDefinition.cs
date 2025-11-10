using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class OpenFileCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.OpenFile";

    [RegisterStaticObject<CommandKeyboardShortcut>]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<OpenFileCommandDefinition>(new KeyGesture(Key.O, KeyModifiers.Control));

    public override string Name => CommandName;

    public override string Text => "Resources.FileOpenCommandText";

    public override string ToolTip => "Resources.FileOpenCommandToolTip";

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Open.png");
}