using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.InternalTest.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class ViewInternalTestToolCommandDefinition : CommandDefinition
{
    public const string CommandName = "View.InternalTestTool";

    public override string Name => CommandName;

    public override string Text => "内部测试Toolbox";

    public override string ToolTip => "内部测试Toolbox的ToolTip";
}