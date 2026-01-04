using Gekimini.Avalonia.Example.Assets.Languages;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Tools;
using Gekimini.Avalonia.Utils.MethodExtensions;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Tools;

public class InternalTestToolViewModel : ToolViewModelBase
{
    public InternalTestToolViewModel() : base(Lang.B.InternalTestToolTitle.ToLocalizedString())
    {
        
    }
}