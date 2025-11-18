using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Toolbox.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewToolboxCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.Toolbox";

    public override string Name => CommandName;

    public override string Text => Resources.ViewToolboxCommandText;

    public override string ToolTip => Resources.ViewToolboxCommandToolTip;
}