using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Framework.Services
{
    public interface IMainView
    {
        IShell Shell { get; }
    }
}
