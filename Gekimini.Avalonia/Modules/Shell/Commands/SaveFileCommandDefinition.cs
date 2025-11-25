using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
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

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.FileSaveCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.FileSaveCommandToolTip));

    public override Uri IconSource => new("avares://Gekimini.Avalonia/Assets/Icons/Save.png");
}