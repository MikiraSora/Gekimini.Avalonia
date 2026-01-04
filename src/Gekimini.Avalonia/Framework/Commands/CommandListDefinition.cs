using System;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils.MethodExtensions;

namespace Gekimini.Avalonia.Framework.Commands;

public abstract class CommandListDefinition : CommandDefinitionBase
{
    public sealed override LocalizedString Text { get; } = "[NotUsed]".ToLocalizedStringByRawText();

    public sealed override LocalizedString ToolTip { get; } = "[NotUsed]".ToLocalizedStringByRawText();

    public sealed override Uri IconSource => null;

    public sealed override bool IsList => true;
}