using Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.ToolboxItems;

[RegisterSingleton<ToolboxItem>]
public class SubItem : ToolboxItem<InternalTestDocumentViewModel>
{
    public override string Category => "Normal";
    public override string Name => "Sub Item";
    public override Uri IconSource => default;
}