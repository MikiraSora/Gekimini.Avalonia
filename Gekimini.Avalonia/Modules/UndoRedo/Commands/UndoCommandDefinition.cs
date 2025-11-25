using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class UndoCommandDefinition : CommandDefinition
{
    public const string CommandName = "Edit.Undo";

    [RegisterStaticObject]
    public static CommandKeyboardShortcut KeyGesture =
        new CommandKeyboardShortcut<UndoCommandDefinition>(new KeyGesture(Key.Z, KeyModifiers.Control));

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.EditUndoCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.EditUndoCommandToolTip));

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Undo.png");
}