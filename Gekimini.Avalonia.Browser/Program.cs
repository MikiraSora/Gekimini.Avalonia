using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Media;
using Dock.Settings;

namespace Gekimini.Avalonia.Browser;

internal sealed class Program
{
    private static Task Main(string[] args)
    {
        return BuildAvaloniaApp()
            .WithInterFont()
            .With(new FontManagerOptions
            {
                FontFallbacks = new[]
                {
                    new FontFallback
                    {
                        FontFamily =
                            new FontFamily(
                                "avares://Gekimini.Avalonia.Browser/Assets/Fonts/NotoSansSC-VariableFont_wght.ttf#Noto Sans SC")
                    }
                }
            })
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<BrowserApp>();
    }
}