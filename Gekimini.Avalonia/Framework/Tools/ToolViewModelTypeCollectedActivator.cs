using Dock.Model.Controls;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Utils;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Framework;

[CollectTypeForActivator(typeof(IToolViewModel))]
public partial class ToolViewModelTypeCollectedActivator : ITypeCollectedActivator<IToolViewModel>
{

}