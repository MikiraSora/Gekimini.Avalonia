using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Settings.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class OpenSettingsCommandDefinition : CommandDefinition
{
    public const string CommandName = "Tools.Options";

    public override string Name => CommandName;

    public override string Text => Resources.ToolsOptionsCommandText;

    public override string ToolTip => Resources.ToolsOptionsCommandToolTip;
}