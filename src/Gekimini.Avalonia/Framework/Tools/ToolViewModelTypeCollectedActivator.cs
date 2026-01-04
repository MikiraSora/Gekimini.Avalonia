using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Framework;

[CollectTypeForActivator(typeof(IToolViewModel))]
public partial class ToolViewModelTypeCollectedActivator : ITypeCollectedActivator<IToolViewModel>
{
}