using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Gekimini.Avalonia.Platforms.Services.MainWindow;

public interface IPlatformMainWindow
{
    bool IsFullScreen { get; set; }
    string Title { get; set; }
    Rect? WindowRect { get; set; }
    WindowIcon Icon { get; set; }
}