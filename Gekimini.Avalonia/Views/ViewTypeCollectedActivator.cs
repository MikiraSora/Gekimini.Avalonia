using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Views
{
    [CollectTypeForActivator(typeof(IView))]
    public partial class ViewTypeCollectedActivator : ITypeCollectedActivator<IView>
    {

    }
}
