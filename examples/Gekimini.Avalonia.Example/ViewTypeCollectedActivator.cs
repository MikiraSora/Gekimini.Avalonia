using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Utils;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Example;

[CollectTypeForActivator(typeof(IView))]
public partial class ViewTypeCollectedActivator : ITypeCollectedActivator<IView>
{
}