using System;
using System.Windows.Input;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems
{
    public abstract class StandardMenuItemViewModel : MenuItemViewModelBase
    {
        public abstract LocalizedString Text { get; }
        public abstract Uri IconSource { get; }
        public abstract string InputGestureText { get; }
        public abstract ICommand Command { get; }
        public abstract bool IsChecked { get; }
        public abstract bool IsVisible { get; }
    }
}