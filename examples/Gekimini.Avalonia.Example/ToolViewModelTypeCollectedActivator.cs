using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Example;

[CollectTypeForActivator(typeof(IToolViewModel))]
public partial class ToolViewModelTypeCollectedActivator : ITypeCollectedActivator<IToolViewModel>
{
}