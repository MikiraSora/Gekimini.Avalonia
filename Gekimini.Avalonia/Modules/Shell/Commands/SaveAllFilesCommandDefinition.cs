using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class SaveAllFilesCommandDefinition : CommandDefinition
{
    public const string CommandName = "File.SaveAllFiles";

    [RegisterStaticObject<CommandKeyboardShortcut>]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<SaveAllFilesCommandDefinition>(new KeyGesture(Key.S,
            KeyModifiers.Control | KeyModifiers.Shift));

    public override string Name => CommandName;

    public override string Text => "Resources.FileSaveAllCommandText";

    public override string ToolTip => "Resources.FileSaveAllCommandToolTip";

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/SaveAll.png");
}