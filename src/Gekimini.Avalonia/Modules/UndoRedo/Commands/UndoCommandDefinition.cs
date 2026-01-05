using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;
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
        ProgramLanguages.B.EditUndoCommandText.ToLocalizedString();

    public override LocalizedString ToolTip { get; } =
        ProgramLanguages.B.EditUndoCommandToolTip.ToLocalizedString();

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Undo.png");
}