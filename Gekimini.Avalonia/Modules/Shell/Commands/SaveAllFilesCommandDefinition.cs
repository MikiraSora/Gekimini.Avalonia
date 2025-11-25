using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
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

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.FileSaveAllCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.FileSaveAllCommandToolTip));

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/SaveAll.png");
}