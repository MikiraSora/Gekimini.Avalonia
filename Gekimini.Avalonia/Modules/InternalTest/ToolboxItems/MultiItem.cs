using System;
using Gekimini.Avalonia.Modules.InternalTest.ViewModels;
using Gekimini.Avalonia.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.InternalTest.ToolboxItems;

[RegisterSingleton<ToolboxItem>]
public class MultiItem : ToolboxItem<InternalTestDocumentViewModel>
{
    public override string Category => "High";
    public override string Name => "Multi Item";
    public override Uri IconSource => default;
}