using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Views;

[CollectTypeForActivator(typeof(IView))]
public partial class ViewTypeCollectedActivator : ITypeCollectedActivator<IView>
{
}