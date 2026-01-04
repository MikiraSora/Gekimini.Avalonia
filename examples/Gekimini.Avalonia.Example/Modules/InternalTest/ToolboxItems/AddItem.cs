using Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.ToolboxItems;

[RegisterSingleton<ToolboxItem>]
public class AddItem : ToolboxItem<InternalTestDocumentViewModel>
{
    public override string Category => "Normal";
    public override string Name => "Add Item";
    public override Uri IconSource => default;
}