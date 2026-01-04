using System.Linq;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Modules.StatusBar.ViewModels;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class IStatusBarEx
{
    public static StatusBarItemViewModel GetApplicationGlobalStatusBarItem(this IStatusBar statusBar)
    {
        return statusBar?.Items?.ElementAtOrDefault(0);
    }
}