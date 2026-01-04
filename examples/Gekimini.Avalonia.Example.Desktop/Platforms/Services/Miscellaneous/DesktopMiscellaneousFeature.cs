using System.Diagnostics;
using Gekimini.Avalonia.Platforms.Services.Miscellaneous;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Desktop.Platforms.Services.Miscellaneous;

[RegisterSingleton<IMiscellaneousFeature>]
public class DesktopMiscellaneousFeature : IMiscellaneousFeature
{
    public void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}