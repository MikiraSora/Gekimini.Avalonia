using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Tools;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Tools;

public class InternalTestToolViewModel : ToolViewModelBase
{
    public InternalTestToolViewModel() : base(LocalizedString.CreateFromRawText(nameof(InternalTestToolViewModel)))
    {
    }
}