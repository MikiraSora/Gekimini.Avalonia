using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Commands;

[RegisterSingleton<ICommandKeyGestureService>]
public class CommandKeyGestureService : ICommandKeyGestureService
{
    private readonly ICommandService _commandService;
    private readonly CommandKeyboardShortcut[] _keyboardShortcuts;

    public CommandKeyGestureService(
        CommandKeyboardShortcut[] keyboardShortcuts,
        ExcludeCommandKeyboardShortcut[] excludeKeyboardShortcuts,
        ICommandService commandService)
    {
        _keyboardShortcuts = keyboardShortcuts
            .Except(excludeKeyboardShortcuts.Select(x => x.KeyboardShortcut))
            .OrderBy(x => x.SortOrder)
            .ToArray();
        _commandService = commandService;
    }

    public void BindKeyGestures(Control uiElement)
    {
        foreach (var keyboardShortcut in _keyboardShortcuts)
            if (keyboardShortcut.KeyGesture != null)
            {
                var command = _commandService.GetTargetableCommand(
                    _commandService.GetCommand(keyboardShortcut.CommandDefinition));
                var gesture = keyboardShortcut.KeyGesture;
                uiElement.KeyBindings.Add(new KeyBinding
                {
                    Command = command,
                    Gesture = gesture
                });
            }
    }

    public KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition)
    {
        var keyboardShortcut = _keyboardShortcuts.FirstOrDefault(x => x.CommandDefinition == commandDefinition);
        return keyboardShortcut != null
            ? keyboardShortcut.KeyGesture
            : null;
    }
}