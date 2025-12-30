using Avalonia.Threading;
using Gekimini.Avalonia.Browser.Utils.Interops;
using Gekimini.Avalonia.Platforms.Services.Miscellaneous;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Browser.Platforms.Services.Miscellaneous;

[RegisterSingleton<IMiscellaneousFeature>]
public class BrowserMiscellaneousFeature : IMiscellaneousFeature
{
    public void OpenUrl(string url)
    {
        Dispatcher.UIThread.Invoke(() => WindowInterop.OpenURL(url), DispatcherPriority.Background);
    }
}