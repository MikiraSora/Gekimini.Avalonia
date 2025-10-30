using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gekimini.Avalonia.Framework
{
    public interface ILayoutItem
    {
        Guid Id { get; }
        /*
        ICommand CloseCommand { get; }
        Uri IconSource { get; }
        */
    }
}
