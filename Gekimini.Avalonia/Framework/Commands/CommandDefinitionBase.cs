using System;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework.Commands
{
    public abstract class CommandDefinitionBase
    {
        public abstract string Name { get; }
        public abstract LocalizedString Text { get; }
        public abstract LocalizedString ToolTip { get; }
        public abstract Uri IconSource { get; }
        public abstract bool IsList { get; }
    }
}