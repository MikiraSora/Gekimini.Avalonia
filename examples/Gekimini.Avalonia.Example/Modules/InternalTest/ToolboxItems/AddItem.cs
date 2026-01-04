using Gekimini.Avalonia.Example.Assets.Languages;
using Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.ToolboxItems;

[RegisterSingleton<ToolboxItem>]
public class AddItem : ToolboxItem<InternalTestDocumentViewModel>
{
    public override LocalizedString Category => "Normal".ToLocalizedStringByRawText();
    public override LocalizedString Name => Lang.B.IncrementValue.ToLocalizedString();
    public override Uri IconSource => default;
}