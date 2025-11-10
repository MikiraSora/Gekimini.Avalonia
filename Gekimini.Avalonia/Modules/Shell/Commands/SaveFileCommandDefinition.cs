using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class SaveFileCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.SaveFile";

    [RegisterStaticObject<CommandKeyboardShortcut>]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture(Key.S, KeyModifiers.Control));

    public override string Name => CommandName;

    public override string Text => "Resources.FileSaveCommandText";

    public override string ToolTip => "Resources.FileSaveCommandToolTip";

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Save.png");
}