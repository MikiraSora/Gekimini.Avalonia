using Dock.Model.Core;

namespace Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

public class LayoutDockWindow
{
    public string Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public bool Topmost { get; set; }

    public string Title { get; set; }
    //public IDockable Owner { get; set; }
    //public IFactory Factory { get; set; }
    //public IRootDock Layout { get; set; }
    //public IHostWindow Host { get; set; }

    public void CopyFrom(IDockWindow layoutWindow)
    {
        Id = layoutWindow.Id;
        Title = layoutWindow.Title;
        Height = layoutWindow.Height;
        Width = layoutWindow.Width;
        X = layoutWindow.X;
        Y = layoutWindow.Y;
        Topmost = layoutWindow.Topmost;
    }

    public void CopyTo(IDockWindow layoutWindow)
    {
        layoutWindow.Id = Id;
        layoutWindow.Title = Title;
        layoutWindow.Height = Height;
        layoutWindow.Width = Width;
        layoutWindow.X = X;
        layoutWindow.Y = Y;
        layoutWindow.Topmost = Topmost;
    }
}