using System;
using System.Windows.Input;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;

namespace Gemini.Framework.Menus
{
    public abstract class MenuDefinitionBase
    {
        public abstract int SortOrder { get; }
        public abstract LocalizedString Text { get; }
        public abstract Uri IconSource { get; }
        public abstract KeyGesture KeyGesture { get; }
        public abstract CommandDefinitionBase CommandDefinition { get; }
    }
}