using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
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

    public override string Text => Resources.EditUndoCommandText;

    public override string ToolTip => Resources.EditUndoCommandToolTip;

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Undo.png");
}