using System;
using System.Windows.Input;
using Gemini.Framework.Menus;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;

public class TextMenuItemViewModel : StandardMenuItemViewModel
{
    private readonly MenuDefinitionBase _menuDefinition;

    public TextMenuItemViewModel(MenuDefinitionBase menuDefinition)
    {
        _menuDefinition = menuDefinition;
    }

    public override string Text => _menuDefinition.Text;

    public override Uri IconSource => _menuDefinition.IconSource;

    public override string InputGestureText =>
        _menuDefinition.KeyGesture == null
            ? string.Empty
            : _menuDefinition.KeyGesture.ToString();

    public override ICommand Command => null;

    public override bool IsChecked => false;

    public override bool IsVisible => true;
}