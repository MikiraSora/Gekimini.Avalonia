using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Utils
{
    public interface ITypeCollectedActivator<T>
    {
        bool TryCreateInstance(IServiceProvider serviceProvider, string fullName, out T obj);
    }
}
