using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class RedoCommandDefinition : CommandDefinition
{
    public const string CommandName = "Edit.Redo";

    [RegisterStaticObject]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<RedoCommandDefinition>(new KeyGesture(Key.Y, KeyModifiers.Control));

    [RegisterStaticObject]
    public static CommandKeyboardShortcut KeyGesture2 =
        new CommandKeyboardShortcut<RedoCommandDefinition>(new KeyGesture(Key.Z,
            KeyModifiers.Control | KeyModifiers.Shift));

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.EditRedoCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(ProgramLanguages.EditRedoCommandToolTip));

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Redo.png");
}