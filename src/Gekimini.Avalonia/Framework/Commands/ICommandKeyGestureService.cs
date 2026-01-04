using Avalonia.Controls;
using Avalonia.Input;

namespace Gekimini.Avalonia.Framework.Commands
{
    public interface ICommandKeyGestureService
    {
        void BindKeyGestures(Control uiElement);
        KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition);
    }
}