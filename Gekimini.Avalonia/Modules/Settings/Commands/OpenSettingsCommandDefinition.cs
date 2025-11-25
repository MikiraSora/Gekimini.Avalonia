using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Settings.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class OpenSettingsCommandDefinition : CommandDefinition
{
    public const string CommandName = "Tools.Options";

    public override string Name => CommandName;

    public override LocalizedString Text { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.ToolsOptionsCommandText));

    public override LocalizedString ToolTip { get; } =
        LocalizedString.CreateFromResourceKey(nameof(Resources.ToolsOptionsCommandToolTip));
}