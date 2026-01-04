using System.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ControlEx
{
    public static Control GetFocusedElement(this Control userControl)
    {
        if (userControl is null)
            return null;

        return userControl
            .GetVisualDescendants()
            .OfType<Control>()         
            .FirstOrDefault(c => c.IsFocused);
    }
}