using System;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;

namespace Gemini.Framework.Menus;

public class TextMenuItemDefinition : MenuItemDefinition
{
    private readonly Uri _iconSource;
    private readonly LocalizedString _text;

    public TextMenuItemDefinition(MenuItemGroupDefinition group, int sortOrder, LocalizedString text,
        Uri iconSource = null)
        : base(group, sortOrder)
    {
        _text = text;
        _iconSource = iconSource;
    }

    public override LocalizedString Text => _text;

    public override Uri IconSource => _iconSource;

    public override KeyGesture KeyGesture => null;

    public override CommandDefinitionBase CommandDefinition => null;
}